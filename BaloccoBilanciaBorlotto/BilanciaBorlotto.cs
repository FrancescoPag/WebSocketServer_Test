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
    public delegate void RunEventDelegate(bool isRunning);
    public class BilanciaBorlotto
    {
        private const int WEB_SOCKET_SERVER_PORT = 81;
        private static Logger logger = LogManager.GetLogger("myLogger");
         
        private static HashSet<UserContext> _bilanciaUsers, _borlottoUsers;
        private static object _lockBilancia, _lockBorlotto;
        private static Thread _thServer, _thBilanciaConsumer, _thBorlottoConsumer;
        private static AutoResetEvent _stopServerEvent;
        private static volatile bool _run, _runBilancia, _runBorlotto;
        private static BilanciaProducer _bilanciaProducer;
        
        public static bool IsRunning { get { return _run; } }
        public static RunEventDelegate RunEvent;  
       
        static BilanciaBorlotto()
        {
            _run = false;
            _bilanciaUsers = new HashSet<UserContext>();
            _borlottoUsers = new HashSet<UserContext>();
            _lockBilancia = new object();
            _lockBorlotto = new object();
            _bilanciaProducer = new BilanciaProducer(new BilanciaSettings());
            _stopServerEvent = new AutoResetEvent(false);
        }

        public static void Main(string[] args)
        {
            Start();
        }

        public static void Start()
        {
            if (_run) return;

            //_bilanciaUsers = new HashSet<UserContext>();
            //_borlottoUsers = new HashSet<UserContext>();
            //_lockBilancia = new object();
            //_lockBorlotto = new object();
            //_stopServerEvent = new AutoResetEvent(false);
            _run = true;
            _runBilancia = false;
            _runBorlotto = false;
            //_bilanciaProducer = new BilanciaProducer(new BilanciaSettings());  
            _stopServerEvent.Reset();        

            WebSocketServer wsServer = new WebSocketServer(WEB_SOCKET_SERVER_PORT, IPAddress.Loopback)
            {
                OnConnected = OnConnect,
                OnDisconnect = OnDisconnect,
                OnReceive = OnReceive,
                TimeOut = new TimeSpan(10, 0, 0)
            };

            _thServer = new Thread(() =>
                {
                    Log(LogLevel.Debug, "started");
                    wsServer.Start();
                    _stopServerEvent.WaitOne();
                    wsServer.Stop();
                    Log(LogLevel.Debug, "stopped");
                });
            _thServer.Name = "Listener";
            
            
            Thread thGUI = new Thread(() =>    // Thread "gui"
                {
                    Log(LogLevel.Debug, "started");
                    String s = String.Empty;
                    while (!s.Equals("exit"))
                        s = Console.ReadLine();

                    // chiudi tutto
                    _run = false;
                    RemoveAllBilanciaUsers();
                    RemoveAllBorlottoUsers();

                    _stopServerEvent.Set();
                    Log(LogLevel.Debug, "stopped");
                });
            thGUI.Name = "GUI";
            

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

            thGUI.Start();
            _thServer.Start();
            _thBilanciaConsumer.Start();
            if(RunEvent != null) RunEvent(_run);
            Log(LogLevel.Debug, "BilanciaBorlotto started");

            thGUI.Join();
            _thServer.Join();
            _thBilanciaConsumer.Join();
            Console.ReadLine();
        }

        public static void Stop()
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

        private static void OnConnect(UserContext user)
        {
            //user.
            Log(LogLevel.Info, user.ClientAddress.ToString() + " connected");
        }

        private static void OnReceive(UserContext user)
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

        private static void OnDisconnect(UserContext user)
        {
            Log(LogLevel.Info, user.ClientAddress.ToString() + " disconnected");
            RemoveBorlottoUser(user);
            RemoveBilanciaUser(user);
            //log
        }

        private static bool RemoveBorlottoUser(UserContext user)
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

        private static void AddBorlottoUser(UserContext user)
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

        private static void RemoveAllBorlottoUsers()
        {
            lock (_lockBorlotto)
            {
                _borlottoUsers.Clear();
                _runBorlotto = false;
                Monitor.PulseAll(_lockBorlotto);
                Log(LogLevel.Trace, "removed all borlotto users");
            }
        }

        private static bool RemoveBilanciaUser(UserContext user)
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

        private static void AddBilanciaUser(UserContext user)
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

        private static void RemoveAllBilanciaUsers()
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
