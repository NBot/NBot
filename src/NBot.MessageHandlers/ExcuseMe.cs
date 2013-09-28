using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;

namespace NBot.MessageHandlers
{
    [Tag("Fun", "Excuse")]
    public class ExcuseMe : MessageHandler
    {
        [Respond("excuse me")]
        [Help(Syntax = "nbot excuse me", Description = "Return a developer excuse for you to use.", Example = "nbot excuse me")]
        public void DoExcuseMe(Message mesage, IMessageClient client)
        {
            IEntity user = client.GetUser(mesage.UserId);
            IRestClient httpClient = GetJsonServiceClient("http://developerexcuses.com/");
            var result = httpClient.Get<string>("/");
            Match matches = Regex.Match(result, "<a href=\"/\" .*>(.*)</a>");
            string excuse = matches.Groups[1].Value;
            client.ReplyTo(mesage, string.Format("{0}, your excuse is \"{1}\".", user.Name, excuse));
        }
    }
}