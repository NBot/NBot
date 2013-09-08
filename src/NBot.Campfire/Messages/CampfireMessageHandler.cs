using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core.Extensions;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Campfire.Messages
{
    public class CampfireMessageHandler : IHandleMessages
    {
        private readonly HttpClient _client;
        private readonly IMessagingService _messagingService;

        public CampfireMessageHandler(HttpClient client, IMessagingService messagingService)
        {
            _client = client;
            _messagingService = messagingService;
        }

        [HandleByType("CampfireChangeNameMessage")]
        public dynamic HandleChangeNameMessage(GenericMessage message)
        {
            var body = new { room = new { name = message.Content } };
            var content = new StringContent(body.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage result = _client.PutAsync(string.Format("room/{0}.json", message.RoomId), content).Result;
            return result.Content.ReadAsStringAsync().Result.FromJson();
        }


        [HandleByType("CampfireChangeTopicMessage")]
        public dynamic HandleChangeTopicMessage(GenericMessage message)
        {
            var body = new { room = new { topic = message.Content } };
            var content = new StringContent(body.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage result = _client.PutAsync(string.Format("room/{0}.json", message.RoomId), content).Result;
            return result.Content.ReadAsStringAsync().Result.FromJson();
        }

        [HandleByType("CampfireGetAccountMessage")]
        public Account HandleGetAccountMessage(GenericMessage message)
        {
            var result = _client.GetAsync("account.json")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result
                .FromJson<Account>("account");

            result.MessagingService = _messagingService;

            return result;
        }

        [HandleByType("CampfireGetAllRoomsMessage")]
        public List<Room> HandleGetAllRoomsMessage(GenericMessage message)
        {
            var rooms = _client.GetAsync("rooms.json").Result.Content.ReadAsStringAsync().Result.FromJson<List<Room>>("rooms");
            rooms.ForEach(x => x.MessagingService = _messagingService);
            return rooms;
        }

        [HandleByType("CampfireGetMyUserMessage")]
        public User HandleGetMyUserMessage(GenericMessage message)
        {
            var myUser = _client.GetAsync("users/me.json")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result.FromJson<User>("user");

            return myUser;
        }

        [HandleByType("CampfireGetPresenceMessage")]
        public IEnumerable<Room> HandleGetPresenceMessage(GenericMessage message)
        {
            var rooms = _client.GetAsync("presence.json").Result.Content.ReadAsStringAsync().Result.FromJson<List<Room>>("rooms");
            rooms.ForEach(x => x.MessagingService = _messagingService);
            return rooms;
        }

        [HandleByType("CampfireGetRoomMessage")]
        public Room HandleMessage(GenericMessage message)
        {
            HttpResponseMessage result = _client.GetAsync(string.Format("room/{0}.json", message.RoomId)).Result;
            var room = result.Content.ReadAsStringAsync().Result.FromJson<Room>("room");
            room.MessagingService = _messagingService;
            return room;
        }

        [HandleByType("CampfireGetUserMessage")]
        public User HandleGetUserMessage(UserMessage message)
        {
            return _client.GetAsync(string.Format("users/{0}.json", message.UserId)).Result.Content.ReadAsStringAsync().Result.FromJson<User>("user");
        }

        [HandleByType("CampfireJoinRoomMessage")]
        public bool HandleJoinRoomMessag(GenericMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/join.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        [HandleByType("CampfireLeaveRoomMessage")]
        public bool HandleLeaveRoomMessage(GenericMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/leave.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        [HandleByType("CampfireLockRoomMessage")]
        public bool HandleLockRoomMessage(GenericMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/lock.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        [HandleByType("CampfireUnlockRoomMessage")]
        public bool HandleUnlockRoomMessage(GenericMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/unlock.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        [HandleByType("CampfireSpeakMessage")]
        public UserMessage HandleSpeakMessage(GenericMessage message)
        {
            var body = new { message = new { body = message.Content, type = string.Empty } };
            var content = new StringContent(body.ToJson(), Encoding.UTF8, "application/json");
            return _client.PostAsync(string.Format("room/{0}/speak.json", message.RoomId), content)
                .Result.Content.ReadAsStringAsync()
                .Result.FromJson<UserMessage>("message");
        }
    }
}
