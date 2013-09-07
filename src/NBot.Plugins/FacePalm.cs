using System.Net.Http;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class FacePalm : RecieveMessages
    {
        [Help(Syntax = "facepalm",
            Description = "Anytime the word 'facepalm' is detected, a picture of a facepalm will be posted into the room.",
            Example = "facepalm")]
        [RecieveByRegex("facepalm")]
        public void Recieve(IMessage message, IHostAdapter host)
        {
            HttpClient client = CreateHttpClient("http://facepalm.org");
            string imageUrl = client.GetAsync("/img.php").Result.RequestMessage.RequestUri.AbsoluteUri;
            host.ReplyTo(message, imageUrl);
        }
    }
}