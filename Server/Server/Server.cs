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
                // Automatically disposes the following objects,
                // which implement IDisposable.
                using (var client = (TcpClient)clientObject)
                using (var sReader = new StreamReader(client.GetStream()))
                using (var sWriter = new StreamWriter(client.GetStream()))
                {
                    sWriter.AutoFlush = true;

                    HandleNetworkStream(client, sReader, sWriter);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception} \n");
            }

            // Client connection lost.
            Console.WriteLine($"Client removed. Clients connected: {_tcpClients.Count}");
        }

        /// <summary>
        /// Handles the network stream loop.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sReader"></param>
        /// <param name="sWriter"></param>
        private void HandleNetworkStream(TcpClient client, StreamReader sReader, StreamWriter sWriter)
        {
            User user = null;
            var receivedMessage = string.Empty;

            while (!(receivedMessage.StartsWith(Constants.ExitCommand)))
            {
                if (user == null)
                {
                    // Welcomes client through client stream.
                    sWriter.WriteLine($"{TimeNow} [SERVER] Welcome. Please enter your numeric user id.");
                }

                // Attempts to assign message from client stream.
                // Blocks until it receives something.
                receivedMessage = sReader.ReadLine() ?? string.Empty;

                // Writes to server console.
                Console.WriteLine($"From client {client.GetHashCode()}: {receivedMessage}");

                if (user == null)
                {
                    user = AuthenticateUser(receivedMessage, sWriter);
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
                                writer.WriteLine($"{TimeNow} [{user.Name}] {_broadcastMessage}");
                            }

                            _broadcastMessage = string.Empty;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Authenticates a client as an existing user.
        /// </summary>
        /// <param name="requestedId"> The client's submitted </param>
        /// <param name="sWriter"></param>
        /// <returns></returns>
        private User AuthenticateUser(string requestedId, StreamWriter sWriter)
        {
            User user = null;
            // Integer conversion successful.
            if (int.TryParse(requestedId, out var id))
            {
                user = _userManager.FindUserById(id);

                if (user != null)
                {
                    WriteLineAsServer($"Successfully authenticated as {user.Name}.", sWriter);
                }
            }
            else
            {
                WriteLineAsServer("You didn't enter a numeric value.", sWriter);
            }

            return user;
        }

        private static void WriteLineAsServer(string message, StreamWriter sWriter)
        {
            sWriter.WriteLine($"{TimeNow} [SERVER] {message}");
        }

        #endregion
    }
}