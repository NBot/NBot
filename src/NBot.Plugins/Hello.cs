using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Hello : RecieveMessages
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

        [RecieveByRegex("(hello|good( [d'])?ay(e)?)")]
        public void SayHello(IUserMessage message, IMessageAdapter host)
        {
            var user = host.GetUser(message.UserId);
            host.ReplyTo(message, string.Format(GetRandomItem(_hellos), user.Name));
        }


        [RecieveByRegex("(^(good )?m(a|o)rnin(g)?)")]
        public void SayGoodMorning(IUserMessage message, IMessageAdapter host)
        {
            var user = host.GetUser(message.UserId);
            host.ReplyTo(message, string.Format(GetRandomItem(_mornings), user.Name));
        }

    }
}
