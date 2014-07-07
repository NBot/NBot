using System.Collections.Generic;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack;

namespace NBot.MessageHandlers
{
    [Tag("Fun", "Joke", "Chuck", "Meme")]
    public class ChuckNorris : MessageHandler
    {
        [Help(Syntax = "chuck norris me <Optional: Name>",
            Description =
                "When the command is issued, a Chuck Norris joke will be returned. If a name is provided, it will replace 'Chuck Norris' with the name provided.",
            Example = "chuck norris me John")]
        [Respond("(chuck norris)( me)?{{name}}")]
        public void ChuckNorrisJoke(Message message, IMessageClient client, string name)
        {
            IRestClient jsonClient = GetJsonServiceClient("http://api.icndb.com/jokes/");

            RootObject result;

            if (!string.IsNullOrEmpty(name))
            {
                result = jsonClient.Get<string>("random?firstName=" + name + "&lastName=").FromJson<RootObject>();
            }
            else
            {
                result = jsonClient.Get<string>("random").FromJson<RootObject>();
            }

            client.ReplyTo(message, result.Value.Joke);
        }

        private class RootObject
        {
            public string Type { get; set; }
            public Value Value { get; set; }
        }

        private class Value
        {
            public int Id { get; set; }
            public string Joke { get; set; }
            public List<string> Categories { get; set; }
        }
    }
}