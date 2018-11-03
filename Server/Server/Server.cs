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

        public Server(int port)
        {
            var ipAddress = IPAddress.Any;
            // Initializes and starts the server.
            _tcpListener = new TcpListener(ipAddress, port);
            _tcpListener.Start();

            Console.WriteLine($"Started server at IP Address: {ipAddress}, Port: {port} \n");

            ListenForConnections();
        }

        // Main thread listens for incoming connections.
        public void ListenForConnections()
        {
            // Listening loop.
            while (true)
            {
                Console.WriteLine("Listening for client connection request...");

                // Waits for client connection request (blocking call).
                var newClient = _tcpListener.AcceptTcpClient();

                // Client found.
                Console.WriteLine("Connected to client.");

                // Creates new thread for established connection.
                var clientThread = new Thread(HandleClient);
                clientThread.Start(newClient);
            }
        }

        // New thread is created for every established connection.
        private void HandleClient(object clientObject)
        {
            Console.WriteLine("New client connection thread started.");
            var client = (TcpClient)clientObject;

            // Gets a stream object for reading and writing.
            var networkStream = client.GetStream();
        }
    }
}