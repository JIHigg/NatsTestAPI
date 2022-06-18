using NatsTestCore.Events;

namespace NatsTestCore.Interfaces
{
    public interface IMessageSub
    {
        void IncomingChatMessage(object sender, NewMessageEvent e);
        void Subscribe(IMessagePub publisher);
    }
}
