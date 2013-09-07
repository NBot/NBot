using System.Net.Http;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class GetMyUserMessage : EmptyMessage
    {
    }

    public class GetMyUserMessageHandler : IMessageHandler<GetMyUserMessage, User>
    {
        private readonly HttpClient _client;

        public GetMyUserMessageHandler(HttpClient client)
        {
            _client = client;
        }

        #region IMessageHandler<GetMyUserMessage,User> Members

        public User HandleMessage(GetMyUserMessage message)
        {
            var myUser = _client.GetAsync("users/me.json")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result.FromJson<User>("user");

            return myUser;
        }

        #endregion
    }
}