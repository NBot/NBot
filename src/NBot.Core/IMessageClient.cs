using System.Collections.Generic;

namespace NBot.Core
{
    public interface IMessageClient
    {
        void Send(string message, string roomId, string user = null);
        void Broadcast(string message);
        IEntity GetUser(string userId);
        IEntity GetRoom(string roomId);
        List<IEntity> GetAllRooms();
        List<IEntity> GetUsersInRoom(string roomId);
    }
}