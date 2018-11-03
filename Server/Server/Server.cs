using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    internal class Server
    {
        private readonly TcpListener _tcpListener = null;

        private string Time => $"[{System.DateTime.Now.ToString("HH:mm:ss")}]";

        #region Constructor
        public Server(int port)
        {
            // Allows any available IP Address to be used.
            var ipAddress = IPAddress.Any;

            // Initializes and starts the server.
            _tcpListener = new TcpListener(ipAddress, port);
            _tcpListener.Start();

            Console.WriteLine($"{Time} Started server. Listening on any IP Address, Port: {port} \n");

            ListenForConnections();
        }
        #endregion

        // Main thread listens for incoming connections.
        public void ListenForConnections()
        {
            // Listening loop.
            while (true)
            {
                Console.WriteLine("...Listening for client connection request... \n");

                // Waits for client connection request (blocking call).
                var newClient = _tcpListener.AcceptTcpClient();

                // Client found.
                Console.WriteLine($"{Time} Connected to client.");

                // Creates new thread for established connection.
                var clientThread = new Thread(HandleClient);
                clientThread.Start(newClient);
            }
        }

        // New thread is created for every established connection.
        private void HandleClient(object clientObject)
        {
            Console.WriteLine($"{Time} New client connection thread started.");
            var client = (TcpClient)clientObject;

            // Gets a stream object for reading and writing.
            var networkStream = client.GetStream();
        }
    }
}