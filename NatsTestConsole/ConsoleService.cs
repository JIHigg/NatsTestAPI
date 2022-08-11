using NatsTestCore.Events;
using NatsTestCore.Interfaces;
using NatsTestCore.Objects;
using NatsTestCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NatsTestConsole
{
    public class ConsoleService : IMessageSub, IDisposable
    {
        private readonly ChatSettings _chatSettings;
        private readonly NatsService _natsService;
        public List<Message> Messages { get; set; }

        public ConsoleService(ChatSettings chatSettings)
        {
            _chatSettings = chatSettings;
            _natsService = new NatsService(_chatSettings);
            Subscribe(_natsService);

            _natsService.SubChannel(_chatSettings.ChatSubject);

            Messages = new List<Message>();
        }

        /// <summary>
        /// Sends Message to NATS Server
        /// </summary>
        /// <param name="username"></param>
        public void StartChat(string username)
        {
            string input = "";

            Console.Clear();
            Console.WriteLine("[Enter 'q' to Quit]");
            Console.WriteLine("\nEnter your message: ");

            input = Console.ReadLine();
            if(input.Length == 1 && input.ToLower() == "q")
            {
                Environment.Exit(0);
            }

            while(input != string.Empty)
            {
                _natsService.PublishMessage(_chatSettings.ChatSubject, new Message
                {
                    ID = username,
                    SentTime = DateTime.Now,
                    Text = input
                });
            }
        }



        public void Dispose()
        {
            _natsService.Dispose();
        }

        /// <summary>
        /// Displays incoming messages from NATS Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IncomingChatMessage(object sender, NewMessageEvent e)
        {
            Messages.Add(e.Message);

            Console.Clear();
            DisplayChat();
            Console.WriteLine("Enter Your Message: ");
        }

        /// <summary>
        /// Displays most recent 10 messages from Subject Chat
        /// </summary>
        private void DisplayChat()
        {
            Console.WriteLine("[Enter 'q' to Quit]\n");

            if (Messages.Count > 10)
                Console.WriteLine("...");

            //Sort by datetime and take latest ten
            var conversation = Messages
                .OrderByDescending(m => m.SentTime)
                .Take(10)
                .OrderBy(m => m.SentTime);

            foreach (var message in conversation)
                Console.WriteLine(message);

        }

        public void Subscribe(IMessagePub publisher)
        {
            publisher.ReceivedNewMessage += IncomingChatMessage;
        }
    }
}
