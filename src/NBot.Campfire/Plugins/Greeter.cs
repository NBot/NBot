using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Campfire.Plugins
{
    public class Greeter : RecieveMessages
    {
        [RecieveByType("EnterMessage")]
        public void RecieveEnterMessage(EnterMessage message, IHostAdapter host)
        {
            var user = host.GetUser(message.UserId);
            host.ReplyTo(message, string.Format("Welcome, {0}", user.Name));
        }

        [RecieveByType("LeaveMessage")]
        public void RecieveLeaveMessage(LeaveMessage message, IHostAdapter host)
        {
            var user = host.GetUser(message.UserId);
            host.ReplyTo(message, string.Format("Good bye, {0}", user.Name));
        }
    }
}
