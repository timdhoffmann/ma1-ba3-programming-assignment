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
        private readonly UserManager _userManager = new UserManager(10);

        private static string TimeNow => $"[{System.DateTime.Now:HH:mm:ss}]";
        private string _broadcastMessage = string.Empty;

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

        /// <summary>
        /// Main thread listens for incoming connections.
        /// </summary>
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

        /// <summary>
        /// Handles an established connection with a single client.
        /// </summary>
        /// <param name="clientObject"> The client to manage the connection with. </param>
        private void HandleClient(object clientObject)
        {
            Console.WriteLine($"{TimeNow} New client connection thread started. \n");

            try
            {
                using (var client = (TcpClient)clientObject)
                using (var sReader = new StreamReader(client.GetStream()))
                using (var sWriter = new StreamWriter(client.GetStream()))
                {
                    sWriter.AutoFlush = true;

                    // Welcomes client through client stream.
                    sWriter.WriteLine($"{TimeNow} [SERVER] Welcome.");

                    var receivedMessage = string.Empty;
                    // Network stream loop.
                    while (!(receivedMessage.StartsWith("exit")))
                    {
                        // Attempts to assign message from client stream.
                        // Blocks until it receives something.
                        receivedMessage = sReader.ReadLine() ?? string.Empty;

                        // Writes to server console.
                        Console.WriteLine($"From client {client.GetHashCode()}: {receivedMessage}");

                        if (!receivedMessage.StartsWith("exit"))
                        {
                            _broadcastMessage = receivedMessage;

                            // Writes to client stream.
                            foreach (var tcpClient in _tcpClients)
                            {
                                var writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                                writer.WriteLine($"{TimeNow} {client.GetHashCode()} {_broadcastMessage}");
                            }

                            _broadcastMessage = string.Empty;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception} \n");
            }

            // Client connection lost.
            // TODO: Need to handle client IDisposable?
            _tcpClients.Remove((TcpClient)clientObject);
            Console.WriteLine($"Client removed. Clients connected: {_tcpClients.Count}");
        }
    }
}