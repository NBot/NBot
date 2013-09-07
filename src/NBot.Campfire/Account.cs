using System;
using System.Collections.Generic;
using System.Linq;
using NBot.Campfire.Messages;
using NBot.Core.Messaging;
using Newtonsoft.Json;

namespace NBot.Campfire
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubDomain { get; set; }
        public string Plan { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZome { get; set; }

        public int Storage { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public IMessagingService MessagingService { get; set; }

        public Room GetRoom(int id)
        {
            return MessagingService.Send<Room>(new GetRoomMessage(id));
        }

        public List<User> GetOnlineUsers()
        {
            List<Room> rooms = GetRooms();
            return rooms.SelectMany(room => room.Users).ToList();
        }

        public List<Room> GetRooms()
        {
            return MessagingService.Send<List<Room>>(new GetAllRoomsMessage());
        }

        public List<Room> GetPresence()
        {
            return MessagingService.Send<List<Room>>(new GetPresenceMessage());
        }

        public User GetUser(int userId)
        {
            return MessagingService.Send<User>(new GetUserMessage(userId));
        }
    }
}