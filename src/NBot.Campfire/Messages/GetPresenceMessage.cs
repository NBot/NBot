using System.Collections.Generic;
using System.Net.Http;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class GetPresenceMessage : EmptyMessage
    {
    }

    public class GetPresenceMessageHandler : IMessageHandler<GetPresenceMessage, IEnumerable<Room>>
    {
        private readonly HttpClient _client;
        private readonly IMessagingService _messagingService;

        public GetPresenceMessageHandler(HttpClient client, IMessagingService messagingService)
        {
            _client = client;
            _messagingService = messagingService;
        }

        #region IMessageHandler<GetPresenceMessage,IEnumerable<Room>> Members

        public IEnumerable<Room> HandleMessage(GetPresenceMessage message)
        {
            var rooms = _client.GetAsync("presence.json").Result.Content.ReadAsStringAsync().Result.FromJson<List<Room>>("rooms");
            rooms.ForEach(x => x.MessagingService = _messagingService);
            return rooms;
        }

        #endregion
    }
}