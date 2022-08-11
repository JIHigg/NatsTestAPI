using NATS.Client;
using System;

namespace NatsTestConsole
{
    /// <summary>
    /// A NATS message including a subject, reply, header, data payload, and subscription information.
    /// </summary>
    public class OldMessage
    {
        private string subject;
        private string reply;
        private byte[] data;
        internal Subscription sub;
        internal MsgHeader header;


        /// <summary>
        /// Gets or sets the payload of the message.
        /// </summary>
        public byte[] Data
        {
            get { return data; }

            set
            {
                if (value == null)
                {
                    this.data = null;
                    return;
                }

                int len = value.Length;
                if (len == 0)
                    this.data = null;
                else
                {
                    this.data = new byte[len];
                    Array.Copy(value, 0, data, 0, len);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the reply subject.
        /// </summary>
        public string Reply
        {
            get { return reply; }
            set { reply = value; }
        }

        /// <summary>
        /// Gets or Sets the subject.
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Msg"/> class with a subject, reply, header, and data.
        /// </summary>
        /// <param name="subject">Subject of the message.</param>
        /// <param name="reply">A reply subject, or <c>null</c>.</param>
        /// <param name="header">Message headers or <c>null</c>.</param>
        /// <param name="data">A byte array containing the message payload.</param>
        public OldMessage(string subject, string reply, MsgHeader header, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("Subject cannot be null, empty, or whitespace.", nameof(subject));
            }

            this.Subject = subject;
            this.Reply = reply;
            this.header = header;
            this.Data = data;
        }

    }
}
