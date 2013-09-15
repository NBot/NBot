using System;
using System.Diagnostics;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class MemeGenerator : MessageHandler
    {
        [Help(Syntax = "Brace yourself <text>",
            Description = "Ned Stark braces for <text>",
            Example = "Brace yourself users")]
        [Respond("brace yourself (.+)")]
        public void BraceYourself(Message message, IMessageClient client, string[] matches)
        {
            MemGen(message, client, "http://i.imgur.com/cOnPlV7.jpg", "Brace Yourself", matches[1]);
        }

        [Help(Syntax = "All your <text> are belong to <text>",
            Description = "All your <text> are belong to <text>",
            Example = "All your bases are belong to us")]
        [Respond("(All your .*) (are belong to .*)")]
        public void AllYourBelongTo(Message message, IMessageClient client, string[] matches)
        {
            MemGen(message, client, "http://i.imgur.com/gzPiQ8R.jpg", matches[1], matches[2]);
        }

        [Help(Syntax = "<text> ALL the <things>",
            Description = "Generates ALL THE THINGS",
            Example = "Automate ALL the Things")]
        [Respond("(.*) (ALL the .*)")]
        public void AllTheThings(Message message, IMessageClient client, string[] matches)
        {
            MemGen(message, client, "http://memecaptain.com/all_the_things.jpg", matches[1], matches[2]);
        }

        [Help(Syntax = "I don't always <something> but when i do <text>",
            Description = "Generates The Most Interesting man in the World",
            Example = "I don't always debug but when i do its in production")]
        [Respond("(I DON'?T ALWAYS .*) (BUT WHEN I DO,? .*)")]
        public void DontAlways(Message message, IMessageClient client, string[] matches)
        {
            MemGen(message, client, "http://memecaptain.com/most_interesting.jpg", matches[1], matches[2]);
        }

        [Help(Syntax = "Y U NO <text>",
            Description = "Generates the Y U NO GUY with the bottom caption of <text>",
            Example = "Y U NO Connex")]
        [Respond("Y U NO (.+)")]
        public void WhyYouNo(Message message, IMessageClient client, string[] matches)
        {
            MemGen(message, client, "http://memecaptain.com/y_u_no.jpg", "Y U NO", matches[1]);
        }

        [Help(Syntax = "<text> (SUCCESS|NAILED IT)",
            Description = "Generates success kid with the top caption of <text>",
            Example = "Jonathan NAILED IT")]
        [Respond("(.*)(SUCCESS|NAILED IT.*)")]
        public void NailedIt(Message message, IMessageClient client, string[] matches)
        {
            MemGen(message, client, "http://memecaptain.com/success_kid.jpg", matches[1], matches[2]);
        }

        private void MemGen(Message message, IMessageClient client, string urlString, string text1, string text2)
        {
            try
            {
                var result = GetJsonServiceClient(string.Format("http://memecaptain.com/g?u={0}&t1={1}&t2={2}", urlString, text1, text2))
                    .Get<string>("/");

                string startOfUrl = result.Substring(result.IndexOf("http"));
                string imgUrl = startOfUrl.Substring(0, startOfUrl.IndexOf("\""));
                client.ReplyTo(message, imgUrl);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-MemeGenerator", ex.ToString());
                client.ReplyTo(message, "MemGenerator crashed!");
            }
        }
    }
}