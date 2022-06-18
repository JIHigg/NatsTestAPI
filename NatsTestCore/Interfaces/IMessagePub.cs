using NatsTestCore.Events;
using NatsTestCore.Objects;
using System;

namespace NatsTestCore.Interfaces
{
    public interface IMessagePub
    {
        event EventHandler<NewMessageEvent> ReceivedNewMessage;
        void IncomingMessage(Message message);
    }
}
