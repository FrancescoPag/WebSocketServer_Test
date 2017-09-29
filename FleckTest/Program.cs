using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FleckTest
{
    class Program
    {
        static IWebSocketConnection client;

        static void Main(string[] args)
        {
            Console.WriteLine($"Main thread: {Thread.CurrentThread.ManagedThreadId}");
            string host = "ws://0.0.0.0:";

            WebSocketServer ws = new WebSocketServer(host + "81");
            ws.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Client {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} {socket.ConnectionInfo.Host} [ID:{socket.ConnectionInfo.Id}] connected. [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                    client = socket;
                    Console.WriteLine($"Available: {Program.client.IsAvailable}");
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine($"Client {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} {socket.ConnectionInfo.Host} [ID:{socket.ConnectionInfo.Id}] disconnected. [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                    Console.WriteLine($"Available: {Program.client.IsAvailable}");
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine($"Received {message}.  [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                };
            });

            Console.WriteLine("Press for send");
            Console.ReadLine();
            client.Send("dbasdnad");

            Console.WriteLine("Press for disconnect client");
            Console.ReadLine();
            client.Close();

            Console.WriteLine("Press for close listener");
            Console.ReadLine();
            ws.ListenerSocket.Close();

            Console.WriteLine("Press for port 82");
            Console.ReadLine();
            ws = new WebSocketServer(host + "82");
            ws.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Client {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} {socket.ConnectionInfo.Host} [ID:{socket.ConnectionInfo.Id}] connected. [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                    client = socket;
                    Console.WriteLine($"Available: {Program.client.IsAvailable}");
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine($"Client {socket.ConnectionInfo.ClientIpAddress}:{socket.ConnectionInfo.ClientPort} {socket.ConnectionInfo.Host} [ID:{socket.ConnectionInfo.Id}] disconnected. [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                    Console.WriteLine($"Available: {Program.client.IsAvailable}");
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine($"Received {message}.  [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                };
            });
            Console.ReadLine();
        }
    }
}
