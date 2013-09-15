using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.Plugins
{
    public class FortuneMe : MessageHandler
    {
        [Help(Syntax = "<{0}|{1}> fortune [me]",Description = "Displays a forturne", Example = "nbot fortune me")]
        [Respond("(fortune)( me)?")]
        public void GetMyFortune(Message message, IMessageClient client)
        {
            var body = GetJsonServiceClient("http://www.fortunefortoday.com/getfortuneonly.php").Get<string>("/");

            client.ReplyTo(message, body.Trim());
        }

    }
}
