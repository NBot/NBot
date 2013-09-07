using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class ExcuseMe : RecieveMessages
    {
        [RespondByRegex("excuse me")]
        [Core.Help.Help(Syntax = "nbot excuse me",Description = "Return a developer excuse for you to use.", Example= "nbot excuse me")]
        public void DoExcuseMe(IUserMessage mesage, IHostAdapter host)
        {
            var user = host.GetUser(mesage.UserId);
            var client = CreateHttpClient("http://developerexcuses.com/");
            var result = client.GetAsync("").Result.Content.ReadAsStringAsync().Result;
            var matches = Regex.Match(result, "<a href=\"/\" .*>(.*)</a>");
            var excuse = matches.Groups[1].Value;
            host.ReplyTo(mesage, string.Format("{0}, your excuse is \"{1}\".", user.Name, excuse));
        }
    }
}
