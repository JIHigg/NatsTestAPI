using NatsTestCore.Objects;
using System;

namespace NatsTestConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                //MessagerClient.Run();

                string username = "";
                string subject = "";
                string url = "https://localhost:44310/api/Nats";

                Console.WriteLine("Please Enter a UserName: ");
                username = Console.ReadLine();

                Console.WriteLine("Please Enter a Subject: ");
                subject = Console.ReadLine();

                var chatSettings = new ChatSettings
                {
                    ChatSubject = subject,
                    ChatUrl = url
                };

                using (var chatService = new ConsoleService(chatSettings))
                {
                    chatService.StartChat(username);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
