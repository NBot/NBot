using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Swanson : MessageHandler
    {

        [Help(Syntax = "<{0}|{1}> <ron|ron swanson|swanson> me", Description = "Displays an image of Ron Swanson along with a quote", Example = "nbot ron me")]
        [Respond("(ron|ron swanson|swanson) me")]
        public void SwansonMe(Message message, IMessageClient client)
        {
            var httpclient = GetJsonServiceClient("http://ronsays.tumblr.com");
            var html = httpclient.Get<string>("/random");
            var linkMatch = Regex.Match(html, "<div class=\"stat-media-wrapper\"><a href=\"http://ronsays.tumblr.com/image/(\\d+)\"><img\\ssrc=\"(.*)\"\\salt");
            client.ReplyTo(message, linkMatch.Groups[2].Value);
        }
    }
}
