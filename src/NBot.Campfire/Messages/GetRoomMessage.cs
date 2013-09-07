using System.Net.Http;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class GetRoomMessage : EmptyMessage
    {
        public GetRoomMessage(int roomId)
        {
            RoomId = roomId;
        }

        public int RoomId { get; private set; }
    }

    public class GetRoomMessageHandler : IMessageHandler<GetRoomMessage, Room>
    {
        private readonly HttpClient _client;
        private readonly IMessagingService _messagingService;

        public GetRoomMessageHandler(HttpClient client, IMessagingService messagingService)
        {
            _client = client;
            _messagingService = messagingService;
        }

        #region IMessageHandler<GetRoomMessage,Room> Members

        public Room HandleMessage(GetRoomMessage message)
        {
            HttpResponseMessage result = _client.GetAsync(string.Format("room/{0}.json", message.RoomId)).Result;
            var room = result.Content.ReadAsStringAsync().Result.FromJson<Room>("room");
            room.MessagingService = _messagingService;
            return room;
        }

        #endregion
    }
}