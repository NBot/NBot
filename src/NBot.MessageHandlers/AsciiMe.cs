﻿using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack;

namespace NBot.MessageHandlers
{
    [Tag("Fun","Ascii")]
    public class AsciiMe : MessageHandler
    {
        [Help(Syntax = "ascii me <Phrase>",
            Description = "Given the input phrase, an ASCII drawing will be returned for that phrase.",
            Example = "ascii me Hello")]
        [Respond("ascii( me)? {{query}}")]
        public void GetAscii(Message message, IMessageClient client, string query)
        {
            IRestClient httpClient = GetJsonServiceClient("http://asciime.heroku.com/");
            var result = httpClient.Get<string>("/generate_ascii?s={0}".FormatWith(query));
            client.ReplyTo(message, result);
        }
    }
}