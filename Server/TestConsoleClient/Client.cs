using System;
using System.IO;
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

        #region Constructor
        public Client(string ipAddress, int port)
        {
            IpAddress = ipAddress;
            Port = port;
        }
        #endregion

        public void Connect()
        {
            using (var tcpClient = new TcpClient(IpAddress, Port))
            using (var networkStream = tcpClient.GetStream())
            using (var streamReader = new StreamReader(networkStream))
            using (var streamWriter = new StreamWriter(networkStream))
            {
                Console.Clear();
                streamWriter.AutoFlush = true;

                IsConnected = true;
                Console.WriteLine($"Connected to server at IP Address: {IpAddress}, Port: {Port}");

                // Handles communication.
                var incomingMessage = string.Empty;
                var outgoingMessage = string.Empty;
                while (true)
                {
                    if ((incomingMessage = streamReader.ReadLine()) != null)
                    {
                        Console.WriteLine(incomingMessage);
                    }

                    if ((outgoingMessage = Console.ReadLine()) != null)
                    {
                        streamWriter.Write(outgoingMessage);
                    }
                }
            }
        }
    }
}