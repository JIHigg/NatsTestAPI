using Microsoft.Extensions.Options;
using NatsTestCore.Events;
using NatsTestCore.Interfaces;
using NatsTestCore.Objects;
using NatsTestCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NatsTestAPI.Services
{
    //TODO Explain this
    public class ChatService : IMessageSub, IDisposable
    {
        private readonly NatsService _natsService;
        private readonly List<Message> _messages;
        private readonly string _subject;

        public ChatService(NatsService natsService, IOptions<ChatSettings> options)
        {
            _natsService = natsService;
            Subscribe(_natsService);
            _messages = new List<Message>();
            _subject = options.Value.ChatSubject;
            _natsService.SubChannel(_subject);
        }

        /// <summary>
        /// Assigns datetime to message and publishes it to subject
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            message.SentTime = DateTime.Now;
            _natsService.PublishMessage(_subject, message);
        }

        /// <summary>
        /// Returns list of all messages during Session ordered by datetime
        /// </summary>
        /// <returns></returns>
        public List<Message> GetMessages()
        {
            return _messages.OrderBy(m => m.SentTime).ToList();
        }

        /// <summary>
        /// Implements NATS Dispose method 
        /// </summary>
        public void Dispose()
        {
            _natsService.Dispose();
        }

        /// <summary>
        /// Adds any incoming message to _messages list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IncomingChatMessage(object sender, NewMessageEvent e)
        {
            _messages.Add(e.Message);
        }

        /// <summary>
        /// Invokes event whenever new message comes through new published subject.
        /// </summary>
        /// <param name="publisher"></param>
        public void Subscribe(IMessagePub publisher)
        {
            publisher.ReceivedNewMessage += IncomingChatMessage;
        }
    }
}
