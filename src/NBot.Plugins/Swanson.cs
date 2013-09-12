using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Swanson : RecieveMessages
    {

        [RespondByRegex("(ron|ron swanson|swanson) me")]
        public void SwansonMe(IMessage message, IHostAdapter host)
        {
            var client = CreateHttpClient("http://ronsays.tumblr.com");
            var html = client.GetAsync("/random").Result.Content.ReadAsStringAsync().Result;
            var linkMatch = Regex.Match(html, "<div class=\"stat-media-wrapper\"><a href=\"http://ronsays.tumblr.com/image/(\\d+)\"><img\\ssrc=\"(.*)\"\\salt");
            host.ReplyTo(message, linkMatch.Groups[2].Value);
        }
    }
}
