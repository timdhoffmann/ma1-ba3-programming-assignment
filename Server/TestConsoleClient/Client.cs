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
                Console.WriteLine($"Connected to server at IP Address: {IpAddress}, Port: {Port} \n");

                // Handles communication.
                var outgoingMessage = string.Empty;
                while (!outgoingMessage.Equals("exit"))
                {
                    var incomingMessage = sReader.ReadLine();
                    Console.WriteLine(incomingMessage);

                    // Loops for non-empty user input.
                    while (outgoingMessage == string.Empty)
                    {
                        Console.Write(">> ");
                        outgoingMessage = Console.ReadLine() ?? string.Empty;
                    }

                    sWriter.WriteLine(outgoingMessage);

                    // Resets user input.
                    outgoingMessage = string.Empty;
                }
            }
        }
    }
}