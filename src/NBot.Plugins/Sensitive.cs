using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Sensitive : RecieveMessages
    {
        readonly string[] _messages = new[]
                                 {
                                     "Hey, that stings.",
                                     "Is that tone really necessary?",
                                     "Robots have feelings too, you know.",
                                     "You should try to be nicer.",
                                     "Sticks and stones cannot pierce my anodized exterior, but words *do* hurt me.",
                                     "I'm sorry, I'll try to do better next time.",
                                     "https://p.twimg.com/AoTI6tLCIAAITfB.jpg"
                                 };


        [RespondByRegex("\\b(you|u|is)\\b.*(stupid|buggy|useless|dumb|suck|crap|shitty|idiot)")]
        public void DoHear(IMessage message, IHostAdapter host)
        {
            host.ReplyTo(message, GetRandomItem(_messages));
        }
    }
}