using System;
using NATS.Client;

namespace NatsTestConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                MessagerClient.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
