using System.Collections.Generic;
using System.Net.Http;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class GetAllRoomsMessage : EmptyMessage
    {
    }

    public class GetAllRoomsMessageHandler : IMessageHandler<GetAllRoomsMessage, List<Room>>
    {
        private readonly HttpClient _client;
        private readonly IMessagingService _messagingService;

        public GetAllRoomsMessageHandler(HttpClient client, IMessagingService messagingService)
        {
            _client = client;
            _messagingService = messagingService;
        }

        #region IMessageHandler<GetAllRoomsMessage,List<Room>> Members

        public List<Room> HandleMessage(GetAllRoomsMessage message)
        {
            var rooms = _client.GetAsync("rooms.json").Result.Content.ReadAsStringAsync().Result.FromJson<List<Room>>("rooms");
            rooms.ForEach(x => x.MessagingService = _messagingService);
            return rooms;
        }

        #endregion
    }
}