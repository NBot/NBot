using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public class Message
    {
        public string Channel { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }
}
