using Alchemy;
using Alchemy.Classes;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaloccoBilanciaBorlotto
{
    public class BilanciaBorlotto
    {
        private static Logger logger = LogManager.GetLogger("myLogger");
         
        private HashSet<UserContext> _bilanciaUsers, _borlottoUsers;
        private object _lockBilancia, _lockBorlotto;
        private WebSocketServer _wsServer;
        private int _webSocketServerPort;
        private Thread _thServer, _thBilanciaConsumer, _thBorlottoConsumer;
        private AutoResetEvent _stopServerEvent;
        private volatile bool _run, _runBilancia, _runBorlotto;
        private BilanciaProducer _bilanciaProducer;
        
        public bool IsRunning { get { return _run; } }
        public Action<bool> RunEvent;  
       
        public BilanciaBorlotto(int servePort, BilanciaSettings settings)
        {
            _run = false;
            _bilanciaUsers = new HashSet<UserContext>();
            _borlottoUsers = new HashSet<UserContext>();
            _lockBilancia = new object();
            _lockBorlotto = new object();
            _bilanciaProducer = new BilanciaProducer(settings);
            _stopServerEvent = new AutoResetEvent(false);
            _webSocketServerPort = servePort;

            _wsServer = new WebSocketServer(_webSocketServerPort, IPAddress.Loopback)
            {
                OnConnected = OnConnect,
                OnDisconnect = OnDisconnect,
                OnReceive = OnReceive,
                //FlashAccessPolicyEnabled = false,
                TimeOut = new TimeSpan(10, 0, 0)
            };
        }

        public static void Main(string[] args)
        {
            BilanciaBorlotto bb = new BilanciaBorlotto(81, new BilanciaSettings());
            bb.Start();

            Thread thGUI = new Thread(() =>    // Thread "gui"
            {
                Log(LogLevel.Debug, "started");
                String s = String.Empty;
                while (!s.Equals("exit"))
                    s = Console.ReadLine();

                // chiudi tutto
                bb._run = false;
                bb.RemoveAllBilanciaUsers();
                bb.RemoveAllBorlottoUsers();

                bb._stopServerEvent.Set();
                Log(LogLevel.Debug, "stopped");
            });
            thGUI.Name = "GUI";
            thGUI.Start();

        }

        public void ChangeSettings(int? serverPort = null, BilanciaSettings newSettings = null)
        {
            if(serverPort != null && serverPort != _webSocketServerPort)
            {
                _webSocketServerPort = (int)serverPort;
                if (_run)
                {
                    _stopServerEvent.Set();
                    if (_thServer != null)
                    {
                        _thServer.Join();
                        RunEvent?.Invoke(false);
                        _thServer = null;
                    }
                    _wsServer = new WebSocketServer(_webSocketServerPort, IPAddress.Loopback)
                    {
                        OnConnected = OnConnect,
                        OnDisconnect = OnDisconnect,
                        OnReceive = OnReceive,
                        //FlashAccessPolicyEnabled = false,
                        TimeOut = new TimeSpan(10, 0, 0)
                    };
                    _thServer = new Thread(() =>
                    {
                        _wsServer.Start();
                        RunEvent?.Invoke(true);
                        Log(LogLevel.Debug, $"started (port: {_wsServer.Port.ToString()})");
                        _stopServerEvent.WaitOne();
                        _wsServer.Stop();
                        _wsServer.Dispose();
                        Log(LogLevel.Debug, $"stopped (port: {_wsServer.Port.ToString()})");
                    });
                    _thServer.Name = "Listener";
                    _thServer.Start();
                }
            }
        }

        public void Start()
        {
            if (_run) return;

            _run = true;
            _runBilancia = false;
            _runBorlotto = false;
            _stopServerEvent.Reset();

            _thServer = new Thread(() =>
                {
                    _wsServer.Start();
                    //RunEvent?.Invoke(true);
                    Log(LogLevel.Debug, $"started (port: {_wsServer.Port.ToString()})");
                    _stopServerEvent.WaitOne();
                    _wsServer.Stop();
                    _wsServer.Dispose();
                    //RunEvent?.Invoke(false);
                    Log(LogLevel.Debug, $"stopped (port: {_wsServer.Port.ToString()})");
                });
            _thServer.Name = "Listener";

            _thBilanciaConsumer = new Thread(() =>    // Thread consumatore per bilancia
                {
                    Log(LogLevel.Debug, "started");
                    BilanciaProduct bp = null;
                    String msg = String.Empty;
                    while(_run)
                    {
                        Log(LogLevel.Trace, "running - new cycle");
                        lock (_lockBilancia)
                        {
                            while (_run && !_runBilancia)
                                Monitor.Wait(_lockBilancia);

                            if (!_run)
                                break;

                            //if (!_bilanciaProducer.IsRunning)
                            //    _bilanciaProducer.Start();

                            bp = _bilanciaProducer.GetProduct();
                            if (bp != null)
                            {
                                msg = JsonConvert.SerializeObject(bp);
                                foreach (UserContext user in _bilanciaUsers)
                                    user.Send(msg);
                            }
                        }
                    }
                    //_bilanciaProducer.Stop();
                    Log(LogLevel.Debug, "stopped");
                });
            _thBilanciaConsumer.Name = "BilanciaConsumer";

            _thServer.Start();
            _thBilanciaConsumer.Start();
            if (RunEvent != null) RunEvent(_run);
            Log(LogLevel.Debug, "BilanciaBorlotto started");
        }

        public void Stop()
        {
            if (!_run) return;

            _run = false;
            RemoveAllBilanciaUsers();
            RemoveAllBorlottoUsers();
            _stopServerEvent.Set();
            if(_thServer != null)
            {
                _thServer.Join();
                _thServer = null;
            }
            if (_thBilanciaConsumer != null)
            {
                _thBilanciaConsumer.Join();
                _thBilanciaConsumer = null;
            }
            if(RunEvent != null) RunEvent(_run);
            Log(LogLevel.Debug, " BilanciaBorlotto stopped");
        }

        private void OnConnect(UserContext user)
        {
            //user.
            Log(LogLevel.Info, user.ClientAddress.ToString() + " connected");
        }

        private void OnReceive(UserContext user)
        {
            String msg = user.DataFrame.ToString();
            if(!msg.Contains("\n"))
                Log(LogLevel.Info, user.ClientAddress.ToString() + " sent message " + msg);
            switch (msg.ToLower())
            {
                case "bilancia":
                    RemoveBorlottoUser(user);
                    AddBilanciaUser(user);
                    break;
                case "borlotto":
                    RemoveBilanciaUser(user);
                    AddBorlottoUser(user);
                    break;
            }
            //log
        }

        private void OnDisconnect(UserContext user)
        {
            Log(LogLevel.Info, user.ClientAddress.ToString() + " disconnected");
            RemoveBorlottoUser(user);
            RemoveBilanciaUser(user);
            //log
        }

        private bool RemoveBorlottoUser(UserContext user)
        {
            bool ret = false;
            lock (_lockBorlotto)
            {
                ret = _borlottoUsers.Remove(user);
                if(_runBorlotto && _borlottoUsers.Count == 0)
                {
                    //stop prducer
                    _runBorlotto = false;
                    Monitor.PulseAll(_lockBorlotto);
                    Log(LogLevel.Trace, "removed LAST borlotto user " + user.ClientAddress.ToString());
                }
                else if (ret)
                    Log(LogLevel.Trace, "removed borlotto user " + user.ClientAddress.ToString());
            }
            return ret;
        }

        private void AddBorlottoUser(UserContext user)
        {
            lock (_lockBorlotto)
            {
                _borlottoUsers.Add(user);
                if (!_runBorlotto)
                {
                    //start producer
                    _runBorlotto = true;
                    Monitor.PulseAll(_lockBorlotto);
                    Log(LogLevel.Trace, "added FIRST borlotto user " + user.ClientAddress.ToString());
                }
                else
                    Log(LogLevel.Trace, "added borlotto user " + user.ClientAddress.ToString());
            }
        }

        private void RemoveAllBorlottoUsers()
        {
            lock (_lockBorlotto)
            {
                _borlottoUsers.Clear();
                _runBorlotto = false;
                Monitor.PulseAll(_lockBorlotto);
                Log(LogLevel.Trace, "removed all borlotto users");
            }
        }

        private bool RemoveBilanciaUser(UserContext user)
        {
            bool ret = false;
            lock (_lockBilancia)
            {
                ret = _bilanciaUsers.Remove(user);
                if(_runBilancia && _bilanciaUsers.Count == 0)
                {
                    _bilanciaProducer.Stop();
                    _runBilancia = false;
                    Monitor.PulseAll(_lockBilancia);
                    Log(LogLevel.Trace, "removed LAST bilancia user " + user.ClientAddress.ToString());
                }
                else if (ret)
                    Log(LogLevel.Trace, "removed bilancia user " + user.ClientAddress.ToString());
            }
            return ret;
        }

        private void AddBilanciaUser(UserContext user)
        {
            lock (_lockBilancia)
            {
                _bilanciaUsers.Add(user);
                if (!_runBilancia)
                {
                    _bilanciaProducer.Start();
                    _runBilancia = true;
                    Monitor.PulseAll(_lockBilancia);
                    Log(LogLevel.Trace, "added FIRST bilancia user " + user.ClientAddress.ToString());
                }
                else
                    Log(LogLevel.Trace, "added bilancia user " + user.ClientAddress.ToString());
            }            
        }

        private void RemoveAllBilanciaUsers()
        {
            lock (_lockBilancia)
            {
                _bilanciaUsers.Clear();
                _runBilancia = false;
                Monitor.PulseAll(_lockBilancia);
                Log(LogLevel.Trace, "removed all bilancia users");
            }
        }

        public static void Log(LogLevel level, string msg)
        {
            logger.Log(level, "[Thread: " + (!String.IsNullOrWhiteSpace(Thread.CurrentThread.Name) ? Thread.CurrentThread.Name : Thread.CurrentThread.ManagedThreadId.ToString()) + "] " + msg);
            //Console.WriteLine("[Thread: " + (!String.IsNullOrWhiteSpace(Thread.CurrentThread.Name) ? Thread.CurrentThread.Name : Thread.CurrentThread.ManagedThreadId.ToString()) + "] " + msg);
        }

    }
}
