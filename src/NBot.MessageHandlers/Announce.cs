using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Announce : MessageHandler
    {
        [Help(Syntax = "announce <Message>",
            Description = "The provided messsage will be broadcast to all rooms.",
            Example = "announce Hello Everyone!")]
        [Respond("announce(?! raw) \"?(.*)\"?")]
        public void RoomAnnounce(Message message, IMessageClient client, string[] matches)
        {
            string announcement = matches[1];
            IEntity user = client.GetUser(message.UserId);
            string response = string.Format("{0} says \" {1} \"", user.Name, announcement);
            client.Broadcast(response);
        }

        [Help(Syntax = "announce raw <Message>",
        Description = "The provided messsage will be broadcast to all rooms.",
        Example = "announce raw Hello Everyone!")]
        [Respond("announce raw \"?(.*)\"?")]
        public void RoomAnnounceRaw(Message message, IMessageClient client, string[] matches)
        {
            string announcement = matches[1];
            client.Broadcast(announcement);
        }
    }
}