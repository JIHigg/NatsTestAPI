using NATS.Client;
using NatsTestCore.Events;
using NatsTestCore.Interfaces;
using NatsTestCore.Objects;
using System;
using System.Text.Json;

namespace NatsTestCore.Services
{
    public class NatsService : IMessagePub, IDisposable
    {
        private readonly IEncodedConnection _connection;
        private readonly ChatSettings _chatSettings;

        public event EventHandler<NewMessageEvent> ReceivedNewMessage;

        public NatsService(ChatSettings settings)
        {
            _chatSettings = settings;
            _connection = SetupConnection();
        }

        public IEncodedConnection SetupConnection()
        {
            try
            {
                var connection = new ConnectionFactory().CreateEncodedConnection(_chatSettings.ChatUrl);

                connection.OnSerialize = Serialize;
                connection.OnDeserialize = Deserialize;

                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong connecting to {_chatSettings.ChatUrl}" +
                    $"\n{ex.Message}");

                throw;
            }
        }

        public void SubChannel(string subject)
        {
            _connection.SubscribeAsync(subject, (sender, args) =>
            {
                var incomingMessage = (Message)args.ReceivedObject;

                IncomingMessage(incomingMessage);
            });
        }

        private object Deserialize(byte[] data)
        {
            return JsonSerializer.Deserialize<Message>(data);
        }

        private byte[] Serialize(object message)
        {
            return JsonSerializer.SerializeToUtf8Bytes((Message)message);
        }

        public void PublishMessage(string subject, Message message)
        {
            _connection.Publish(subject, message);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void IncomingMessage(Message message)
        {
            if (ReceivedNewMessage != null)
            {
                ReceivedNewMessage(this, new NewMessageEvent() { Message = message });
            }
        }
    }
}
