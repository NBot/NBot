using System.Net;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;

namespace NBot.MessageHandlers
{
    public class FacePalm : MessageHandler
    {
        [Help(Syntax = "facepalm",
            Description = "Anytime the word 'facepalm' is detected, a picture of a facepalm will be posted into the room.",
            Example = "facepalm")]
        [Hear("facepalm")]
        public void Recieve(Message message, IMessageClient client)
        {
            IRestClient httpClient = GetJsonServiceClient("http://facepalm.org");
            var response = httpClient.Get<HttpWebResponse>("/img.php");
            var imageUrl = response.ResponseUri.AbsoluteUri;
            client.ReplyTo(message, imageUrl);
        }
    }
}