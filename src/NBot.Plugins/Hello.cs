using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.Plugins
{
    public class Hello : MessageHandler
    {
        private readonly string[] _hellos =
        {
            "Well hello there, {0}",
            "Hey {0}, Hello!",
            "Marnin', {0}",
            "Good day, {0}",
            "Good 'aye!, {0}"
        };

        private readonly string[] _mornings =
        {
            "Good morning, {0}",
            "Good morning to you too, {0}",
            "Good day, {0}",
            "Good 'aye!, {0}"
        };

        [Help(Syntax = "<hello|good day>", Description = "NBot responds with a friendly greeting", Example = "hello")]
        [Hear("(hello|good( [d'])?ay(e)?)")]
        public void SayHello(Message message, IMessageClient client)
        {
            var user = client.GetUser(message.UserId);
            client.ReplyTo(message, string.Format(GetRandomItem(_hellos), user.Name));
        }


        [Help(Syntax = "<good morning | morning>", Description = "NBot responds with a friendly greeting", Example = "good morning")]
        [Hear("(^(good )?m(a|o)rnin(g)?)")]
        public void SayGoodMorning(Message message, IMessageClient client)
        {
            var user = client.GetUser(message.UserId);
            client.ReplyTo(message, string.Format(GetRandomItem(_mornings), user.Name));
        }

    }
}
