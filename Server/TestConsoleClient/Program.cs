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

            var isConnected = false;

            // Attempts connecting upon user input.
            while (!isConnected)
            {
                Console.WriteLine($"Press Enter to connect to IP Address: {ipAddress}, Port: {port}... \n");

                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    try
                    {
                        var client = new Client(ipAddress, port);
                        isConnected = true;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"Error: {exception} \n");
                    }
                }
            }

            // Wait to exit.
            Console.ReadKey();
        }
    }
}