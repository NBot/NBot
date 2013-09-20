using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;

namespace NBot.MessageHandlers
{
    public class MemeGenerator : MessageHandler
    {
        [Help(Syntax = "pity <Phrase>",
            Description = "Given the (optional)input phrase, a Mr-T I Pity The Fool will be returned for that phrase.",
            Example = "pity that breaks the build")]
        [Respond("pity( me)?( {{phrase}})?")]
        public void PityTheFool(Message message, IMessageClient client, string phrase)
        {
            var pity = GetRandomPityStyle();

            if (string.IsNullOrEmpty(phrase))
            {
                phrase = GetRandomPity();
            }

            IRestClient httpClient = GetJsonServiceClient("http://version1.api.memegenerator.net/Instance_Create?username=test&password=test&languageCode=en&generatorID=1646&imageID=5353&text0=" + pity + "&text1=" + phrase);
            
            var response = httpClient.Get<MemeGeneratorResponse>("");

            client.ReplyTo(message, response.Result.InstanceImageUrl);
        }

        internal string GetRandomPityStyle()
        {
            var pityVernaculars = new List<string>
                {
                    "I Pity The Fool",
                    "Pity The Fool",
                    "Thou Shalt pity the fool",
                    "Pity thee",
                    "Be pitiful for those",
                    "cast pity towards those"
                };

            var pity = GetRandomItem(pityVernaculars);

            return pity;
        }

        internal string GetRandomPity()
        {
            var thingsToPity = new List<string>
                {
                    "who breaks the build",
                    "who doesn't test before check-in",
                    "who is last to the dessert tray",
                    "who doesn't login to campfire",
                    "who isn't nBot"
                };

            var thingToPity = GetRandomItem(thingsToPity);

            return thingToPity;
        }
            
        [DataContract]
        internal class MemeGeneratorResponse
        {
            [DataMember(Name = "success")]
            public string Success { get; set; }

            [DataMember(Name = "result")]
            public Result Result { get; set; }
        }

        [DataContract]
        internal class Result
        {
            [DataMember(Name = "generatorId")]
            public string GeneratorId { get; set; }

            [DataMember(Name = "instanceImageUrl")]
            public string InstanceImageUrl { get; set; }
        }
    }
}