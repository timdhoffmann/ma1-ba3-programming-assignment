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
            using (var sReader = new StreamReader(networkStream))
            using (var sWriter = new StreamWriter(networkStream))
            {
                Console.Clear();
                sWriter.AutoFlush = true;
                IsConnected = true;
                Console.WriteLine($"Connected to server at IP Address: {IpAddress}, Port: {Port}");

                // Handles communication.
                var message = string.Empty;
                var outgoingMessage = string.Empty;
                while (!message.Equals("exit"))
                {
                    message = Console.ReadLine();
                    // not implemented:
                    // Console.WriteLine();

                    sWriter.WriteLine(message);
                    // not implemented:
                    //sWriter.Flush();

                    var serverMessage = sReader.ReadLine();
                    Console.WriteLine(serverMessage);

                    //if ((incomingMessage = sReader.ReadLine()) != null)
                    //{
                    //    Console.WriteLine(incomingMessage);
                    //}

                    //if ((outgoingMessage = Console.ReadLine()) != null)
                    //{
                    //    sWriter.Write(outgoingMessage);
                    //}
                }
            }
        }
    }
}