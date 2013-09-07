using System.Collections.Generic;
using System.Linq;
using NBot.Core.Messaging;

namespace NBot.Core
{
    public class HostAdapter : IHostAdapter
    {
        private readonly IEnumerable<IMessageAdapter> _messageAdapters;

        public HostAdapter(IEnumerable<IMessageAdapter> messageAdapters)
        {
            _messageAdapters = messageAdapters;
        }

        #region IHostAdapter Members

        public void Broadcast(string content)
        {
            foreach (IMessageAdapter messageAdapter in _messageAdapters)
            {
                messageAdapter.Broadcast(content);
            }
        }

        public void ReplyTo(IMessage message, string content)
        {
            foreach (IMessageAdapter messageAdapter in _messageAdapters)
            {
                messageAdapter.ReplyTo(message, content);
            }
        }

        public void Send(int roomId, string content)
        {
            foreach (IMessageAdapter messageAdapter in _messageAdapters)
            {
                messageAdapter.Send(roomId, content);
            }
        }

        public IEnumerable<IEntity> GetAllRooms()
        {
            return _messageAdapters.SelectMany(messageAdapter => messageAdapter.GetAllRooms());
        }

        public IEnumerable<IEntity> GetPresence()
        {
            return _messageAdapters.SelectMany(messageAdapter => messageAdapter.GetPresence());
        }

        public IEntity GetUser(int userId)
        {
            return _messageAdapters.Select(messageAdapter => messageAdapter.GetUser(userId)).FirstOrDefault(result => result != null);
        }

        public IEnumerable<IEntity> GetUsersInRoom(int roomId)
        {
            return _messageAdapters.SelectMany(ma => ma.GetUsersInRoom(roomId));
        }

        #endregion
    }
}