using System.Net.Http;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class UnlockRoomMessage : EmptyMessage
    {
        public UnlockRoomMessage(int roomId)
        {
            RoomId = roomId;
        }

        public int RoomId { get; private set; }
    }

    public class UnlockRoomMessageHandler : IMessageHandler<UnlockRoomMessage, bool>
    {
        private readonly HttpClient _client;

        public UnlockRoomMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<UnlockRoomMessage,bool> Members

        public bool HandleMessage(UnlockRoomMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/unlock.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        #endregion
    }
}