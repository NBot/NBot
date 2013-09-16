using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;
using NBot.Core.Attributes;

namespace NBot.MessageHandlers
{
    public class BotSnack : MessageHandler
    {
        readonly string[] _responses = new[] 
        {
            "Om nom nom!",
            "That's very nice of you!",
            "Oh thx, have a cookie yourself!",
            "Thank you very much.",
            "Thanks for the treat!"
        };

        [Hear("botsnack")]
        public void HandleBotSnack(Message message, IMessageClient client)
        {
            client.ReplyTo(message, GetRandomItem(_responses));
        }
    }
}
