using System.Collections.Generic;
using NBot.Core.Messaging;

namespace NBot.Core
{
    public interface IMessageAdapter
    {
        void Broadcast(string content);
        void ReplyTo(IMessage message, string content);
        void Send(int roomId, string content);
        IEnumerable<IEntity> GetAllRooms();
        IEnumerable<IEntity> GetPresence();
        IEntity GetUser(int userId);
        IEnumerable<IEntity> GetUsersInRoom(int roomId);
    }
}