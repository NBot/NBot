using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    [Tag("Fun")]
    public class Sensitive : MessageHandler
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

        [Help(Syntax = "<{0}|{1}> <you|u|is> <stupid|buggy|useless|dumb|suck|crap|shitty|idiot>", Description = "Responds to your mean words with a random retort.", Example = "nbot u suck")]
        [Respond("\\b(you|u|is)\\b.*(stupid|buggy|useless|dumb|suck|crap|shitty|idiot)")]
        public void DoHear(Message message, IMessageClient client)
        {
            client.ReplyTo(message, GetRandomItem(_messages));
        }
    }
}