using System.Net.Http;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class JoinRoomMessage : IMessage
    {
        public JoinRoomMessage(int roomId)
        {
            RoomId = roomId;
            Content = string.Empty;
        }

        #region IMessage Members

        public int RoomId { get; set; }

        public string Content { get; set; }

        #endregion
    }

    public class JoinRoomMessageHandler : IMessageHandler<JoinRoomMessage, bool>
    {
        private readonly HttpClient _client;

        public JoinRoomMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<JoinRoomMessage,bool> Members

        public bool HandleMessage(JoinRoomMessage message)
        {
            return _client.PostAsync(string.Format("room/{0}/join.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        #endregion
    }
}