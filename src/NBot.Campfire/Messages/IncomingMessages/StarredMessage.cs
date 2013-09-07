using Newtonsoft.Json;

namespace NBot.Campfire.Messages.IncomingMessages
{
    public class StarredMessage : UserMessage
    {
        [JsonProperty("starred")]
        public bool IsStarred { get; set; }
    }
}