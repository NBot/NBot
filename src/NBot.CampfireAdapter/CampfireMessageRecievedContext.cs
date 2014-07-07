using System;
using NBot.Core;
using ServiceStack;

namespace NBot.CampfireAdapter
{
    public class CampfireMessageRecievedContext
    {
        private readonly Action<Message> _messageRecieved;

        public CampfireMessageRecievedContext(string message, Action<Message> messageRecieved)
        {
            _messageRecieved = messageRecieved;
            Message = BuildMessage(message.FromJson<CampfireMessage>());
        }

        public Message Message { get; private set; }

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