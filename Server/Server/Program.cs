using System;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("**************");
            Console.WriteLine("*** SERVER ***");
            Console.WriteLine("************** \n");

            const int port = 13000;

            try
            {
                var server = new Server(port);
                server.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}