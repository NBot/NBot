using System.Net.Http;
using System.Text;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class ChangeTopicMessage : IMessage
    {
        public ChangeTopicMessage(int roomId, string topic)
        {
            RoomId = roomId;
            Content = topic;
        }

        #region IMessage Members

        public int RoomId { get; set; }

        public string Content { get; set; }

        #endregion
    }

    public class ChangeTopicMessageHandler : IMessageHandler<ChangeTopicMessage, object>
    {
        private readonly HttpClient _client;

        public ChangeTopicMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<ChangeTopicMessage,object> Members

        public dynamic HandleMessage(ChangeTopicMessage message)
        {
            var body = new {room = new {topic = message.Content}};
            var content = new StringContent(body.ToJson(), Encoding.UTF8, "application/json");
            HttpResponseMessage result = _client.PutAsync(string.Format("room/{0}.json", message.RoomId), content).Result;
            return result.Content.ReadAsStringAsync().Result.FromJson();
        }

        #endregion
    }
}