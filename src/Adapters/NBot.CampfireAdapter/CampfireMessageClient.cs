using System.Collections.Generic;
using System.Linq;
using NBot.Core;
using ServiceStack;

namespace NBot.CampfireAdapter
{
    public class CampfireMessageClient : IMessageClient
    {
        private readonly JsonServiceClient _client;

        public CampfireMessageClient(string token, string account)
        {
            _client = new JsonServiceClient("https://{0}.campfirenow.com".FormatWith(account)) { UserName = token, Password = "X" };
        }

        public void Send(string message, string roomId, string user = null)
        {
            var body = user == null ? message : "@{0} {1}".FormatWith(user, message);
            _client.Post<string>("/room/{0}/speak.json".FormatWith(roomId), new { message = new { body } });
        }

        public void Broadcast(string message)
        {
            foreach (var room in GetAllRooms())
            {
                Send(message, room.Id);
            }
        }

        public IEntity GetUser(string userId)
        {
            return _client.Get<CampfireUserWrapper>("/users/{0}.json".FormatWith(userId)).User;
        }

        public IEntity GetRoom(string roomId)
        {
            return _client.Get<CampfireRoom>("/room/{0}.json".FormatWith(roomId));
        }

        public List<IEntity> GetAllRooms()
        {
            return _client.Get<CampfireRoomsWrapper>("/rooms.json").Rooms.Cast<IEntity>().ToList();
        }

        public List<IEntity> GetUsersInRoom(string roomId)
        {
            var room = _client.Get<CampfireRoomWrapper>("/room/{0}.json".FormatWith(roomId)).Room;

            return room.users.Cast<IEntity>().ToList();
        }
    }
}