using System;

namespace NBot.Core
{
    public class Message
    {
        public string Channel { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }


        public Message CloneWithNewContent(string content)
        {
            return new Message()
            {
                Channel = Channel,
                RoomId = RoomId,
                UserId = UserId,
                Type = Type,
                Content = content
            };
        }
    }
}