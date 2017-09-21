using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaloccoBilanciaBorlotto
{
    #region Utils
    public enum Tire
    {
        ANT_SX = 0,
        ANT_DX = 1,
        POST_SX = 2,
        POST_DX = 3
    }

    public class FifoList : LinkedList<double>
    {
        private int maxSize;
        public int MaxSize { get { return maxSize; } set { maxSize = value; while (Count > MaxSize) RemoveLast(); } }
        public bool IsFull { get { return MaxSize == Count; } }
        public double Average { get { return Math.Round(this.Average(), 1); } }

        public FifoList(int maxSize)
        {
            this.MaxSize = maxSize;
        }

        public void Insert(double item)
        {
            AddFirst(item);
            while (Count > MaxSize)
                RemoveLast();
        }
    }

    public class BilanciaSettings
    {
        public int NUMERO_LETTURE = 3;
        public int TIMER_MS = 200;

        public double CORREZIONE_ANT_SX = 1.0;
        public double CORREZIONE_ANT_DX = 1.0;
        public double CORREZIONE_POST_SX = 1.0;
        public double CORREZIONE_POST_DX = 1.0;

        public int PORTA_COM = 1;
        public int BAUD_RATE = 9600;
        public Parity PARITY_BIT = Parity.None;
        public int DATA_BITS = 8;
        public StopBits STOP_BITS = StopBits.One;
    }
    #endregion

    #region Shared
    public class BilanciaProduct
    {
        public double ant_sx, ant_dx, post_sx, post_dx;     // nomi che verranno serializzati in JSON

        public BilanciaProduct(double antSx, double antDx, double postSx, double postDx)
        {
            this.ant_sx = antSx;
            this.ant_dx = antDx;
            this.post_sx = postSx;
            this.post_dx = postDx;
        } 
    }

    public class BilanciaProductQueue
    {
        private volatile bool _newProduct;
        private volatile bool _isClosed;
        private double _antSx, _antDx, _postSx, _postDx;
        private readonly object _lock;
        private AutoResetEvent _newProductEvent;


        public BilanciaProductQueue()
        {
            _newProduct = false;
            _isClosed = false;
            _lock = new object();
            _newProductEvent = new AutoResetEvent(false);
        }

        public void SetProduct(double antSx, double antDx, double postSx, double postDx)
        {
            if (_isClosed) return;

            lock(_lock)
            {
                this._antSx = antSx;
                this._antDx = antDx;
                this._postSx = postSx;
                this._postDx = postDx;
                _newProduct = true;
                _newProductEvent.Set();
                BilanciaBorlotto.Log(LogLevel.Trace, "bilancia product queue: new product set");
            }
        }

        public void Close()
        {
            BilanciaBorlotto.Log(LogLevel.Trace, "bilancia product queue: closed");
            _isClosed = true;
            _newProduct = false;
            _newProductEvent.Set();
        }

        public void Open()
        {
            BilanciaBorlotto.Log(LogLevel.Trace, "bilancia product queue: opened");
            _isClosed = false;
        }


        public BilanciaProduct GetProduct()
        {
            BilanciaBorlotto.Log(LogLevel.Trace, "bilancia product queue: trying to get product (_newProduct " + _newProduct + ")");
            while (!_newProduct && !_isClosed)
                _newProductEvent.WaitOne();

            if (_isClosed)
                return null;
            
            lock (_lock)
            {
                _newProduct = false;
                BilanciaBorlotto.Log(LogLevel.Trace, "bilancia product queue: product has been got");
                return new BilanciaProduct(_antSx, _antDx, _postSx, _postDx);
            }
        }       
    }
    #endregion

    #region Producer
    public class BilanciaProducer
    {
        private const int SERIAL_PORT_BUFFER_SIZE = 33;
        private char[] _readBuffer;
        private FifoList[] _lastReads;
        private SerialPort _serialPort;
        private BilanciaProductQueue _productQueue;
        private BilanciaSettings _settings;
        private volatile bool _isStopped;
        private Thread _thread;
        public bool IsRunning { get { return !_isStopped; } }

        private Random rand = new Random();

        public BilanciaProducer(BilanciaSettings settings)
        {
            this._readBuffer = new char[SERIAL_PORT_BUFFER_SIZE + 1];
            this._lastReads = new FifoList[4];
            for (int i = 0; i < _lastReads.Length; ++i)
                _lastReads[i] = new FifoList(settings.NUMERO_LETTURE);
            this._serialPort = new SerialPort("COM" + settings.PORTA_COM.ToString(), settings.BAUD_RATE, settings.PARITY_BIT, settings.DATA_BITS, settings.STOP_BITS);
            this._isStopped = true;
            this._productQueue = new BilanciaProductQueue();
            this._settings = settings;
        }

        public void Start()
        {
            BilanciaBorlotto.Log(LogLevel.Debug, "trying to start bilancia producer");
            if (_isStopped)
            {
                _isStopped = false;
                _productQueue.Open();
                _thread = new Thread(() =>
                    {
                        BilanciaBorlotto.Log(LogLevel.Debug, "started");
                        while (!_isStopped)
                        {
                            //Produce();
                            PseudoProduce();
                            //await Task.Delay(_settings.TIMER_MS * 10);
                            Thread.Sleep(_settings.TIMER_MS * 10);
                        }
                        for(int i = 0; i < _lastReads.Length; ++i)
                            _lastReads[i].Clear();
                        BilanciaBorlotto.Log(LogLevel.Debug, "stopped");
                        _thread = null;
                    });
                _thread.Name = "BilanciaProducer";
                _thread.Start();
            }
        }

        private void Produce()
        {
            BilanciaBorlotto.Log(LogLevel.Trace, "bilancia is producing");
            try
            {
                _serialPort.Open();
                Array.Clear(_readBuffer, 0, SERIAL_PORT_BUFFER_SIZE + 1);

                int readCount = 0;
                _serialPort.Write("#01A\r");
                while (readCount < SERIAL_PORT_BUFFER_SIZE && _readBuffer[readCount] != '\r' && !_isStopped)
                    _readBuffer[readCount++] = (char)_serialPort.ReadChar();

                _serialPort.Close();

                if (!_isStopped && readCount >= 32)
                {
                    _lastReads[(int)Tire.ANT_SX].Insert(_settings.CORREZIONE_ANT_SX * int.Parse(new string(_readBuffer, 4, 6)));
                    _lastReads[(int)Tire.ANT_DX].Insert(_settings.CORREZIONE_ANT_DX * int.Parse(new string(_readBuffer, 11, 6)));
                    _lastReads[(int)Tire.POST_SX].Insert(_settings.CORREZIONE_POST_SX * int.Parse(new string(_readBuffer, 18, 6)));
                    _lastReads[(int)Tire.POST_DX].Insert(_settings.CORREZIONE_POST_DX * int.Parse(new string(_readBuffer, 25, 6)));

                    if (_lastReads[0].IsFull && _lastReads[1].IsFull && _lastReads[2].IsFull && _lastReads[3].IsFull)
                        _productQueue.SetProduct(_lastReads[(int)Tire.ANT_SX].Average, _lastReads[(int)Tire.ANT_DX].Average, _lastReads[(int)Tire.POST_SX].Average, _lastReads[(int)Tire.POST_DX].Average);
                }
            }
            catch (Exception ex)
            {
                BilanciaBorlotto.Log(LogLevel.Error, "Error while bilancia was producing: " + ex.Message);
            }
            finally
            {
                try { if (_serialPort.IsOpen) _serialPort.Close(); } catch (Exception) { }
            }
        }

        private void PseudoProduce()
        {
            BilanciaBorlotto.Log(LogLevel.Trace, "bilancia is pseudo-producing");
            if (!_isStopped)
            {
                _lastReads[(int)Tire.ANT_SX].Insert(_settings.CORREZIONE_ANT_SX * rand.Next(80,120));
                _lastReads[(int)Tire.ANT_DX].Insert(_settings.CORREZIONE_ANT_DX * rand.Next(180,220));
                _lastReads[(int)Tire.POST_SX].Insert(_settings.CORREZIONE_POST_SX * rand.Next(280,320));
                _lastReads[(int)Tire.POST_DX].Insert(_settings.CORREZIONE_POST_DX * rand.Next(380,420));

                if (_lastReads[0].IsFull && _lastReads[1].IsFull && _lastReads[2].IsFull && _lastReads[3].IsFull)
                    _productQueue.SetProduct(_lastReads[(int)Tire.ANT_SX].Average, _lastReads[(int)Tire.ANT_DX].Average, _lastReads[(int)Tire.POST_SX].Average, _lastReads[(int)Tire.POST_DX].Average);
            }
        }

        public void Stop()
        {
            BilanciaBorlotto.Log(LogLevel.Debug, "stopping bilancia producer");
            _isStopped = true;
            _productQueue.Close();
            //_thread.Join();
            _thread = null;
        }

        public BilanciaProduct GetProduct()
        {
            return _productQueue.GetProduct();
        }
    }
    #endregion


}
