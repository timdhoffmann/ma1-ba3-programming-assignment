using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    internal class Server
    {
        // Allows any available IP Address to be used.
        private readonly IPAddress _ipAddress = IPAddress.Any;
        private readonly int _port;
        private readonly TcpListener _tcpListener = null;
        private readonly HashSet<TcpClient> _tcpClients = new HashSet<TcpClient>();

        private static string TimeNow => $"[{System.DateTime.Now:HH:mm:ss}]";

        #region Constructor
        public Server(int port)
        {
            _port = port;
            _tcpListener = new TcpListener(_ipAddress, port);
        }
        #endregion

        public void Start()
        {
            _tcpListener.Start();
            Console.WriteLine($"{TimeNow} Started server. Listening on every available IP Address, Port: {_port} \n");

            ListenForConnections();
        }

        // Main thread listens for incoming connections.
        public void ListenForConnections()
        {
            // Listening loop.
            while (true)
            {
                Console.WriteLine($"{_tcpClients.Count} clients connected... Listening for connection request... \n");

                // Waits for client connection request (blocking call).
                var newClient = _tcpListener.AcceptTcpClient();

                // Client found.
                Console.WriteLine($"{TimeNow} Connected to client.");

                // Creates new thread for established connection.
                var clientThread = new Thread(HandleClient);
                clientThread.Start(newClient);
            }
        }

        // New thread is created for every established connection.
        private void HandleClient(object clientObject)
        {
            var client = (TcpClient)clientObject;
            Console.WriteLine($"{TimeNow} New client connection thread started.");

            _tcpClients.Add(client);

            var message = string.Empty;

            // Wraps a stream object for reading data.
            using (var streamReader = new StreamReader(client.GetStream()))
            {
            }

            // Wraps a stream object for writing data.
            //using (var streamWriter = new StreamWriter(client.GetStream()))
            //{
            //}
        }

        private void Broadcast()
        {
            // TODO: implementation.
            // Iterates over client list.

            // Sends message to all clients.
        }
    }
}