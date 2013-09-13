using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public interface IMessage
    {
        string Channel { get; set; }
        string RoomId { get; set; }
        string UserId { get; set; }
        string Type { get; set; }
        string Content { get; set; }
    }
}
