using System.Net.Http;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class GetUserMessage : EmptyMessage
    {
        public GetUserMessage(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }

    public class GetUserMessageHandler : IMessageHandler<GetUserMessage, User>
    {
        private readonly HttpClient _client;

        public GetUserMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<GetUserMessage,User> Members

        public User HandleMessage(GetUserMessage message)
        {
            return _client.GetAsync(string.Format("users/{0}.json", message.UserId)).Result.Content.ReadAsStringAsync().Result.FromJson<User>("user");
        }

        #endregion
    }
}