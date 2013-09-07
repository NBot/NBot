using System.Net.Http;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class LeaveRoomMessage : EmptyMessage
    {
        public LeaveRoomMessage(int roomId)
        {
            RoomId = roomId;
        }

        public int RoomId { get; private set; }
    }

    public class LeaveRoomMessageHandler : IMessageHandler<LeaveRoomMessage, bool>
    {
        public LeaveRoomMessageHandler(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; private set; }

        #region IMessageHandler<LeaveRoomMessage,bool> Members

        public bool HandleMessage(LeaveRoomMessage message)
        {
            return Client.PostAsync(string.Format("room/{0}/leave.json", message.RoomId), new StringContent("")).Result.IsSuccessStatusCode;
        }

        #endregion
    }
}