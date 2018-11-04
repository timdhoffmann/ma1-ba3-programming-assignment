using System;

namespace TestConsoleClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Test Console Client. \n");

            const string ipAddress = "127.0.0.1";
            const int port = 13000;
            var client = new Client(ipAddress, port);

            // Loops for user input to start connection.
            while (!client.IsConnected)
            {
                try
                {
                    Console.WriteLine($"Press Enter to connect to IP Address: {ipAddress}, Port: {port}... \n");

                    // Checks for the specific input to start connection.
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        client.Connect();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Error: \n {exception} \n");
                }
            }
        }
    }
}