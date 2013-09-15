using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.Plugins
{
    public class CalmDown : MessageHandler
    {
        [Help(Syntax = "nbot manatee or calm or calm me", Description = "Post a picture of a calming manatee.", Example = "nbot manatee")]
        [Respond("manatee|calm( me)?")]
        public void RespondTo(Message message, IMessageClient client)
        {
            Manatee(message, client);
        }

        [Hear("calm down|simmer down|that escalated quickly")]
        [Help(Syntax = "calm down or simmer down or that escalated quickly somewhere in the message", Description = "Post a picture of a calming manatee.", Example = "You need to simmer down now!!")]
        public void Recieve(Message message, IMessageClient client)
        {
            Manatee(message, client);
        }

        private void Manatee(Message message, IMessageClient client)
        {
            client.ReplyTo(message, string.Format("http://calmingmanatee.com/img/manatee{0}.jpg", GetRandomNumber(1, 30)));
        }
    }
}