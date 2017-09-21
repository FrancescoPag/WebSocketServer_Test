using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Alchemy.Classes;
using Alchemy;
using System.Threading;
using Newtonsoft.Json;

namespace WebSocketServer_Test
{
    enum Mode
    {
        Bilancia = 0,
        Borlotto = 1
    }
    class Server
    {
        static int clientCount = 0;
        static Dictionary<UserContext, int> connectedUsers = new Dictionary<UserContext, int>();
        static Random random = new Random();
        static Object _lock = new object();
        //static UserContext user = null;

        static void Main(string[] args)
        {
            var aServer = new WebSocketServer(81, IPAddress.Loopback)
            {
                OnConnected = OnConnected,
                OnDisconnect = OnDisconnect,
                OnReceive = OnReceive,
                OnSend = OnSend,
                TimeOut = new TimeSpan(0, 5, 0)
            };

            Console.WriteLine("Listening on port 81 [Thread:" + Thread.CurrentThread.ManagedThreadId + "]");
            new Thread(new ThreadStart(() =>
            {
                aServer.Start();
                Console.WriteLine("Server started [Thread:" + Thread.CurrentThread.ManagedThreadId + "]");
                var command = string.Empty;
                while (command != "exit")
                {
                    command = Console.ReadLine();
                    //if (user != null)
                    //    user.Send(command);
                }

                aServer.Stop();
            })).Start();
        }

        static void OnConnected(UserContext aContext)
        {
            lock(connectedUsers)
            {
                Console.WriteLine("Client Connection From : " + aContext.ClientAddress.ToString() + " [ID:" + clientCount + "] [Thread:" + Thread.CurrentThread.ManagedThreadId + "]");// [HostName: " + Dns.GetHostEntry(aContext.ClientAddress.ToString().).HostName + "]");
                connectedUsers.Add(aContext, clientCount++);
                Console.WriteLine(Dns.GetHostEntry(((IPEndPoint)aContext.ClientAddress).Address.ToString()).HostName);
                //clientCount++;
            }
            //Task.Run(() => SendTimeAsync(aContext));
            //Echo(aContext);
            //user = aContext;

            Task.Run(() => SendWeights(aContext));
        }

        static void OnSend(UserContext aContext)
        {
            Console.WriteLine("Msg sent [Thread:" + Thread.CurrentThread.ManagedThreadId + "]");
        }

        static void OnReceive(UserContext aContext)
        {
            try
            {
                //var json = ;
                Console.WriteLine("MsgReceived: " + aContext.DataFrame.ToString() + "[Thread:" + Thread.CurrentThread.ManagedThreadId + "]");
                //dynamic obj = JsonConvert.DeserializeObject(json);
                //Console.WriteLine((string)obj);
            }
            catch (Exception e) // Bad JSON! For shame.
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }



            static async Task printAsync(String s)
        {
            lock (_lock) { Console.WriteLine(s); };
        }

        static void OnDisconnect(UserContext aContext)
        {
            lock (connectedUsers)
            {
                int id;
                if(connectedUsers.TryGetValue(aContext, out id))
                    Console.WriteLine("Client Disconnect : " + aContext.ClientAddress.ToString() + " [ID:" + id + "] [Thread:" + Thread.CurrentThread.ManagedThreadId + "]");
                else
                    Console.WriteLine("Client Disconnect : " + aContext.ClientAddress.ToString() + " [ID: NULL] [Thread:" + Thread.CurrentThread.ManagedThreadId + "]");
                connectedUsers.Remove(aContext);
            }
            //user = null;
        }

        static async Task SendTimeAsync(UserContext user)
        {
            while (true)
            {
                lock (connectedUsers)
                {
                    int id;
                    if (connectedUsers.TryGetValue(user, out id)) {
                        user.Send("{\"ID\":\"" + id + "\", \"time\":\"" + DateTime.Now.ToString() + "\"}");
                        //user.Send(DateTime.Now.ToString());
                    }
                    else return;
                }
                Thread.Sleep(1000);
            }
        }

        static async Task SendWeights(UserContext user)
        {
            String s;
            while (true)
            {
                lock (connectedUsers)
                {
                    int id;
                    if (connectedUsers.TryGetValue(user, out id))
                    {
                        //s = "{\"ID\":\"" + id + "\", \"ant_sx\":\"" + (0.1*random.Next(1500,10000)) + "\", \"ant_dx\":\"" + (0.1 * random.Next(1500, 10000)) + "\", \"post_sx\":\"" + (0.1 * random.Next(1500, 10000)) + "\", \"post_dx\":\"" + (0.1 * random.Next(1500, 10000)) + "\"}";
                        s = JsonConvert.SerializeObject(new Weights(id, random));
                        user.Send(s);
                        Task.Run(() => printAsync(s));
                    }
                    else return;
                }
                Thread.Sleep(1000);
            }
        }

        static void Echo(UserContext user)
        {
            string echo = String.Empty;
            while(true)
            {
                echo = Console.ReadLine();
                if (echo.Equals("quit"))
                    return;
                else
                    user.Send(echo);
            }
        }
    }

    class Weights
    {
        public int ID;
        public double ant_sx, ant_dx, post_sx, post_dx;

        public Weights(int ID, Random rand)
        {
            this.ID = ID;
            ant_sx = Math.Round(0.1 * rand.Next(1500, 10000), 1);
            ant_dx = Math.Round(0.1 * rand.Next(1500, 10000), 1);
            post_sx = Math.Round(0.1 * rand.Next(1500, 10000), 1);
            post_dx = Math.Round(0.1 * rand.Next(1500, 10000), 1);
        }
    }
}
