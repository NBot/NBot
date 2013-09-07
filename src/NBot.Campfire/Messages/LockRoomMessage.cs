using System.Net.Http;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class LockRoomMessage : EmptyMessage
    {
        public LockRoomMessage(int roomId)
        {
            RoomId = roomId;
        }

        public int RoomId { get; private set; }
    }

    public class LockRoomMessageHandler : IMessageHandler<LockRoomMessage, bool>
    {
        private readonly HttpClient _client;

        public LockRoomMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<LockRoomMessage,bool> Members

        public bool HandleMessage(LockRoomMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/lock.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        #endregion
    }
}