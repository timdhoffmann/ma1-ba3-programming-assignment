using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace TestConsoleClient
{
    internal class Client
    {
        private readonly TcpClient _tcpClient = null;

        #region Constructor
        public Client(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(ipAddress, port);

            Console.WriteLine($"Connected to server at IP Address: {ipAddress}, Port: {port}");

            HandleCommunication();
        }
        #endregion

        private void HandleCommunication()
        {
            Console.WriteLine("Handling communication...");
        }
    }
}