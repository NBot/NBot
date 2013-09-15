using System.Collections.Generic;
using System.Linq;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public class MessageFilterClient : IMessageClient
    {
        private readonly List<IMessageFilter> _filters;
        private readonly IMessageClient _innerMessageClient;

        public MessageFilterClient(IMessageClient innerMessageClient, List<IMessageFilter> filters)
        {
            _innerMessageClient = innerMessageClient;
            _filters = filters;
        }

        public void Send(string message, string roomId, string user = null)
        {
            if (_filters.Any(filter => filter.FilterMessage(ref message)))
            {
                return;
            }

            _innerMessageClient.Send(message, roomId, user);
        }

        public void Broadcast(string message)
        {
            if (_filters.Any(filter => filter.FilterMessage(ref message)))
            {
                return;
            }

            _innerMessageClient.Broadcast(message);
        }

        public IEntity GetUser(string userId)
        {
            return _innerMessageClient.GetUser(userId);
        }

        public IEntity GetRoom(string roomId)
        {
            return _innerMessageClient.GetRoom(roomId);
        }

        public List<IEntity> GetAllRooms()
        {
            return _innerMessageClient.GetAllRooms();
        }

        public List<IEntity> GetUsersInRoom(string roomId)
        {
            return _innerMessageClient.GetUsersInRoom(roomId);
        }
    }
}