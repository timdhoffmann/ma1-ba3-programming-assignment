﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    internal class Server
    {
        #region Fields
        // Allows any available IP Address to be used.
        private readonly IPAddress _ipAddress = IPAddress.Any;
        private readonly int _port;
        private readonly TcpListener _tcpListener = null;
        private readonly HashSet<TcpClient> _tcpClients = new HashSet<TcpClient>();
        private readonly UserManager _userManager = new UserManager();

        private static string TimeNow => $"[{System.DateTime.Now:HH:mm:ss}]";

        private string _broadcastMessage = string.Empty;
        #endregion

        #region Constructors

        public Server(int port)
        {
            _port = port;
            _tcpListener = new TcpListener(_ipAddress, port);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            _userManager.DisplayRegisteredUsers();

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
        #endregion

        #region Private Methods
        /// <summary>
        /// Main thread.
        /// Listens for incoming connections.
        /// </summary>
        private void ListenForConnections()
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

                    var receivedMessage = string.Empty;

                    HandleNetworkStream(receivedMessage, sReader, sWriter, client);
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

        /// <summary>
        /// Network stream loop.
        /// </summary>
        /// <param name="receivedMessage"></param>
        /// <param name="sReader"></param>
        /// <param name="client"></param>
        private void HandleNetworkStream(string receivedMessage, StreamReader sReader, StreamWriter sWriter, TcpClient client)
        {
            var isAuthenticated = false;
            User user = null;

            while (!(receivedMessage.StartsWith(Constants.ExitCommand)))
            {
                if (!isAuthenticated)
                {
                    // Welcomes client through client stream.
                    sWriter.WriteLine($"{TimeNow} [SERVER] Welcome. Please enter your numeric user id.");
                }

                // Attempts to assign message from client stream.
                // Blocks until it receives something.
                receivedMessage = sReader.ReadLine() ?? string.Empty;

                // Writes to server console.
                Console.WriteLine($"From client {client.GetHashCode()}: {receivedMessage}");

                if (!isAuthenticated)
                {
                    // Integer conversion successful.
                    if (int.TryParse(receivedMessage, out var id))
                    {
                        user = _userManager.FindUserById(id);
                        isAuthenticated = (user != null);

                        if (isAuthenticated)
                        {
                            sWriter.WriteLine($"{TimeNow} [SERVER] Successfully authenticated as {user.Name}.");
                        }
                    }
                    else
                    {
                        sWriter.WriteLine($"{TimeNow} [SERVER] You didn't enter a numeric value.");
                    }
                }
                // User is authenticated. Handle messages.
                else
                {
                    // Filters for special commands.
                    switch (receivedMessage)
                    {
                        case Constants.ExitCommand:
                            // do something.
                            break;

                        default:
                            // Default behavior.
                            _broadcastMessage = receivedMessage;

                            // Writes to all client streams.
                            foreach (var tcpClient in _tcpClients)
                            {
                                var writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
                                writer.WriteLine($"{TimeNow} {client.GetHashCode()} {_broadcastMessage}");
                            }

                            _broadcastMessage = string.Empty;
                            break;
                    }
                }
            }
        }
        #endregion
    }
}