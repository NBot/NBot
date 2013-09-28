using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;
using ServiceStack.Text;

namespace NBot.CampfireAdapter
{
    public class CampfireMessageRecievedContext
    {
        private readonly Action<Message> _messageRecieved;
        public Message Message { get; private set; }

        public CampfireMessageRecievedContext(string message, Action<Message> messageRecieved)
        {
            _messageRecieved = messageRecieved;
            Message = BuildMessage(message.FromJson<CampfireMessage>());
        }

        public void ProcessMessage()
        {
            _messageRecieved(Message);
        }

        private Message BuildMessage(CampfireMessage message)
        {
            return new Message
            {
                Channel = "Campfire",
                RoomId = message.room_id,
                UserId = message.user_id,
                Content = message.body ?? string.Empty,
                Type = message.type
            };
        }
    }
}
