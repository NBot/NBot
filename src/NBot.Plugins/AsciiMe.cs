using System.Net;
using System.Net.Http;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class AsciiMe : RecieveMessages
    {
        [Help(Syntax = "ascii me <Phrase>",
            Description = "Given the input phrase, an ASCII drawing will be returned for that phrase.",
            Example = "ascii me Hello")]
        [RespondByRegex("ascii( me)? (.+)")]
        public void GetAscii(IMessage message, IHostAdapter host, string[] matches)
        {
            string query = matches[2];
            HttpClient client = CreateHttpClient(string.Format("http://asciime.heroku.com/generate_ascii?s={0}", WebUtility.HtmlEncode(query)));
            string result = client.GetAsync("").Result.Content.ReadAsStringAsync().Result;
            host.ReplyTo(message, result);
        }
    }
}