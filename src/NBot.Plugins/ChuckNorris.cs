using System.Linq;
using System.Net;
using System.Net.Http;
using NBot.Core;
using NBot.Core.Extensions;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class ChuckNorris : RecieveMessages
    {
        [Help(Syntax = "chuck norris me <Optional: Name>",
            Description = "When the command is issued, a Chuck Norris joke will be returned. If a name is provided, it will replace 'Chuck Norris' with the name provided.",
            Example = "chuck norris me John")]
        [RespondByRegex("(chuck norris)( me )?(.*)")]
        public void ChuckNorrisJoke(IMessage message, IHostAdapter host, string[] matches)
        {
            HttpClient client = CreateHttpClient("http://api.icndb.com/jokes/");

            dynamic result;

            if (matches.Count() == 4)
            {
                string user = matches[3];
                result = client.GetAsync("random?firstName=" + user + "&lastName=")
                    .Result.Content.ReadAsStringAsync()
                    .Result.FromJson();
            }
            else
            {
                result = client.GetAsync("random")
                    .Result.Content.ReadAsStringAsync()
                    .Result.FromJson();
            }

            string joke = WebUtility.HtmlDecode(result.value.joke.ToString());
            host.ReplyTo(message, joke);
        }
    }
}