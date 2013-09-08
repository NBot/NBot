using System;
using NBot.Core.Messaging;
using Newtonsoft.Json;

namespace NBot.Campfire.Messages.IncomingMessages
{
    public class RoomMessage : GenericMessage
    {
        public RoomMessage()
        {
            Content = string.Empty;
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        #region IMessage Members

        [JsonProperty("body")]
        public string Content { get; set; }

        [JsonProperty("room_id")]
        public int RoomId { get; set; }

        #endregion
    }
}