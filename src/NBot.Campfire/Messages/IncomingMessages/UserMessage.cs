using NBot.Core.Messaging;
using Newtonsoft.Json;

namespace NBot.Campfire.Messages.IncomingMessages
{
    public class UserMessage : RoomMessage, IUserMessage
    {
        #region IUserMessage Members

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        #endregion
    }
}