using NatsTestCore.Objects;
using System;

namespace NatsTestCore.Events
{
    public class NewMessageEvent : EventArgs
    {
        public Message Message { get; set; }
    }
}
