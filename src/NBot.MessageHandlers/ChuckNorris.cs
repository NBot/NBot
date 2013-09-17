using System.Collections.Generic;
using System.Linq;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;
using ServiceStack.Text;

namespace NBot.MessageHandlers
{
    public class ChuckNorris : MessageHandler
    {
        [Help(Syntax = "chuck norris me <Optional: Name>",
            Description = "When the command is issued, a Chuck Norris joke will be returned. If a name is provided, it will replace 'Chuck Norris' with the name provided.",
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

            client.ReplyTo(message, result.value.joke);
        }

        private class RootObject
        {
            public string type { get; set; }
            public Value value { get; set; }
        }

        private class Value
        {
            public int id { get; set; }
            public string joke { get; set; }
            public List<string> categories { get; set; }
        }
    }
}