using System;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class CalmDown : RecieveMessages
    {
        [Help(Syntax = "nbot manatee or calm or calm me", Description = "Post a picture of a calming manatee.", Example = "nbot manatee")]
        [RespondByRegex("manatee|calm( me)?")]
        public void RespondTo(IMessage message, IHostAdapter host)
        {
            Manatee(message, host);
        }

        [RecieveByRegex("calm down|simmer down|that escalated quickly")]
        [Help(Syntax = "calm down or simmer down or that escalated quickly somewhere in the message", Description = "Post a picture of a calming manatee.", Example = "You need to simmer down now!!")]
        public void Recieve(IMessage message, IHostAdapter host)
        {
            Manatee(message, host);
        }

        private void Manatee(IMessage message, IHostAdapter host)
        {
            int number = (int) Math.Floor(new Random().NextDouble()*30) + 1;

            host.ReplyTo(message, string.Format("http://calmingmanatee.com/img/manatee{0}.jpg", number));
        }
    }
}