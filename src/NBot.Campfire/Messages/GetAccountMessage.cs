using System.Net.Http;
using NBot.Core.Extensions;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public class GetAccountMessage : EmptyMessage
    {
    }

    public class GetAccountMessageHandler : IMessageHandler<GetAccountMessage, Account>
    {
        private readonly HttpClient _client;
        private readonly IMessagingService _messagingService;

        public GetAccountMessageHandler(HttpClient client, IMessagingService messagingService)
        {
            _client = client;
            _messagingService = messagingService;
        }

        #region IMessageHandler<GetAccountMessage,Account> Members

        public Account HandleMessage(GetAccountMessage message)
        {
            var result = _client.GetAsync("account.json")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result
                .FromJson<Account>("account");

            result.MessagingService = _messagingService;

            return result;
        }

        #endregion
    }
}