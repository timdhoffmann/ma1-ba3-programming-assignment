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

            var server = new Server(13000);

            //Console.ReadKey(true);
        }
    }
}