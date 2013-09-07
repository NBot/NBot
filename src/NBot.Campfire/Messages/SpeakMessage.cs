using System.Net.Http;
using System.Text;
using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class SpeakMessage : IMessage
    {
        public SpeakMessage(int roomId, string message)
        {
            RoomId = roomId;
            Content = message;
        }

        #region IMessage Members

        public int RoomId { get; set; }
        public string Content { get; set; }

        #endregion
    }

    public class SpeakMessageHandler : IMessageHandler<SpeakMessage, UserMessage>
    {
        private readonly HttpClient _client;

        public SpeakMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<SpeakMessage,UserMessage> Members

        public UserMessage HandleMessage(SpeakMessage message)
        {
            var body = new {message = new {body = message.Content, type = string.Empty}};
            var content = new StringContent(body.ToJson(), Encoding.UTF8, "application/json");
            return _client.PostAsync(string.Format("room/{0}/speak.json", message.RoomId), content)
                .Result.Content.ReadAsStringAsync()
                .Result.FromJson<UserMessage>("message");
        }

        #endregion
    }
}