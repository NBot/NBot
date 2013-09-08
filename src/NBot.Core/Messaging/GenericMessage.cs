using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Core.Messaging
{
    public class GenericMessage : IMessage
    {
        public virtual int RoomId { get; set; }
        public virtual string Content { get; set; }
        public virtual string MessageType { get; set; }
    }
}
