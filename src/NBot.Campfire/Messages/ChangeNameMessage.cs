using System.Net.Http;
using System.Text;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class ChangeNameMessage : IMessage
    {
        public ChangeNameMessage(int roomId, string name)
        {
            RoomId = roomId;
            Content = name;
        }

        #region IMessage Members

        public int RoomId { get; set; }

        public string Content { get; set; }

        #endregion
    }

    public class ChangeNameMessageHandler : IMessageHandler<ChangeNameMessage, object>
    {
        private readonly HttpClient _client;

        public ChangeNameMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<ChangeNameMessage,object> Members

        public dynamic HandleMessage(ChangeNameMessage message)
        {
            var body = new {room = new {name = message.Content}};
            var content = new StringContent(body.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage result = _client.PutAsync(string.Format("room/{0}.json", message.RoomId), content).Result;
            return result.Content.ReadAsStringAsync().Result.FromJson();
        }

        #endregion
    }
}