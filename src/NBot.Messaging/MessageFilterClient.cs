using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Messaging.MessageFilters;

namespace NBot.Messaging
{
    public class MessageFilterClient : IMessageClient
    {
        private readonly IMessageClient _innerMessageClient;
        private readonly List<IMessageFilter> _filters;

        public MessageFilterClient(IMessageClient innerMessageClient, List<IMessageFilter> filters)
        {
            _innerMessageClient = innerMessageClient;
            _filters = filters;
        }

        public void SendMessage(Message message)
        {
            if (_filters.Any(filter => filter.FilterMessage(message)))
            {
                return;
            }

            _innerMessageClient.SendMessage(message);
        }

        public IEntity GetUser(string userId)
        {
            return _innerMessageClient.GetUser(userId);
        }

        public IEntity GetRoom(string roomId)
        {
            return _innerMessageClient.GetRoom(roomId);
        }
    }
}
