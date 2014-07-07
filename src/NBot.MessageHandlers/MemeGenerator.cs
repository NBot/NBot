using System.Collections.Generic;
using System.Runtime.Serialization;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack;

namespace NBot.MessageHandlers
{
    [Tag("Fun", "Meme")]
    public class MemeGenerator : MessageHandler
    {
        [Help(Syntax = "FindMemeGenerator <Url>",
            Description = "Given the Url get MemeGenerator.net details. Just needed for implementing new memes",
            Example = "FindMemeGenerator Mr-T")]
        [Respond("FindMemeGenerator( {{url}})")]
        public void FindMemeGenerator(Message message, IMessageClient client, string url)
        {
            IRestClient httpClient =
                GetJsonServiceClient(
                    string.Format(
                        "http://version1.api.memegenerator.net/Generator_Select_ByUrlNameOrGeneratorID?generatorID=&urlName={0}",
                        url));

            var response = httpClient.Get<string>("");

            client.ReplyTo(message, response);
        }

        [Help(Syntax = "pity <Phrase>",
            Description = "Given the (optional)input phrase, a Mr-T I Pity The Fool will be returned for that phrase.",
            Example = "pity that breaks the build")]
        [Respond("pity( me)?( {{phrase}})?")]
        public void PityTheFool(Message message, IMessageClient client, string phrase)
        {
            string pity = GetRandomItem(new List<string>
            {
                "I Pity The Fool",
                "Pity The Fool",
                "Thou Shalt pity the fool",
                "Pity thee",
                "Be pitiful for those",
                "cast pity towards those"
            });

            if (string.IsNullOrEmpty(phrase))
            {
                phrase = GetRandomItem(new List<string>
                {
                    "who breaks the build",
                    "who doesn't test before check-in",
                    "who is last to the dessert tray",
                    "who doesn't login to campfire",
                    "who isn't nBot"
                });
            }

            MemeGen(message, client, "1646", "5353", pity, phrase);
        }

        [Help(Syntax = "courage <Phrase 1>, <Phrase 2>",
            Description = "Given the input phrases, a courage wolf will be returned.",
            Example = "courage mosquito bites you, eat it and take back your blood")]
        [Respond("courage( me)?( {{phrase1}})?(, {{phrase2}}?)")]
        public void CourageWolf(Message message, IMessageClient client, string phrase1, string phrase2)
        {
            MemeGen(message, client, "303", "24", phrase1, phrase2);
        }

        [Help(Syntax = "insanity <Phrase 1>, <Phrase 2>",
            Description = "Given the input phrases, an insanity wolf will be returned.",
            Example = "insanity rob a bank, burn the money")]
        [Respond("insanity( me)?( {{phrase1}})?(, {{phrase2}}?)")]
        public void InsanityWolf(Message message, IMessageClient client, string phrase1, string phrase2)
        {
            MemeGen(message, client, "45", "20", phrase1, phrase2);
        }

        private void MemeGen(Message message, IMessageClient client, string generatorId, string imageId, string text0,
            string text1)
        {
            IRestClient httpClient =
                GetJsonServiceClient(
                    string.Format(
                        "http://version1.api.memegenerator.net/Instance_Create?username=test&password=test&languageCode=en&generatorID={0}&imageID={3}&text0={1}&text1={2}",
                        generatorId,
                        text0,
                        text1,
                        imageId));

            var response = httpClient.Get<MemeGeneratorResponse>("");

            client.ReplyTo(message, response.Result.InstanceImageUrl);
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