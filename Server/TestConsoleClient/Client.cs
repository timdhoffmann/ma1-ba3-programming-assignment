using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace TestConsoleClient
{
    internal class Client
    {
        public string IpAddress { get; }
        public int Port { get; }
        public bool IsConnected { get; private set; } = false;

        private readonly TcpClient _tcpClient = null;

        #region Constructor
        public Client(string ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
            _tcpClient = new TcpClient();
        }
        #endregion

        public void Connect()
        {
            try
            {
                _tcpClient.Connect(IpAddress, Port);
                IsConnected = true;
                Console.WriteLine($"Connected to server at IP Address: {IpAddress}, Port: {Port}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error, could not connect to server: \n {exception} \n");
            }

            if (IsConnected)
            {
                HandleCommunication();
            }
        }

        private void HandleCommunication()
        {
            Console.WriteLine("Handling communication...");
        }
    }
}