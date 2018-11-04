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
            try
            {
                _tcpListener.Start();
                Console.WriteLine(
                    $"{TimeNow} Started server. Listening on every available IP Address, Port: {_port} \n");
                ListenForConnections();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                _tcpListener?.Stop();
            }
        }

        // Main thread listens for incoming connections.
        public void ListenForConnections()
        {
            // Listening loop.
            while (true)
            {
                Console.WriteLine($"{_tcpClients.Count} clients connected... Listening for connection request... \n");

                // Waits for client connection request (blocking call).
                // TODO: Handle disposal of TcpClient (maybe move to HandleClient thread?)
                var newClient = _tcpListener.AcceptTcpClient();

                // Client found.
                Console.WriteLine($"{TimeNow} Connected to client.");

                _tcpClients.Add(newClient);

                // Creates new thread for established connection.
                var clientThread = new Thread(HandleClient);
                clientThread.Start(newClient);
            }
        }

        // New thread is created for every established connection.
        private void HandleClient(object clientObject)
        {
            Console.WriteLine($"{TimeNow} New client connection thread started.");

            try
            {
                using (var client = (TcpClient)clientObject)
                using (var networkStream = client.GetStream())
                using (var sReader = new StreamReader(networkStream))
                using (var sWriter = new StreamWriter(networkStream))
                {
                    sWriter.AutoFlush = true;

                    var message = "[SERVER] Connected to server";

                    // Network stream loop.
                    while ((message != null) && (!message.StartsWith("exit")))
                    {
                        // Writes to local console.
                        Console.WriteLine($"From client: {message}");

                        // Writes to client stream.
                        sWriter.WriteLine($"{TimeNow} {message}");

                        // Reads from client stream.
                        message = sReader.ReadLine();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                // TODO: Remove client from list when losing connection.
            }
        }
    }
}