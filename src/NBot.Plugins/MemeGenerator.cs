using System;
using System.Diagnostics;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class MemeGenerator : RecieveMessages
    {
        [Help(Syntax = "Brace yourself <text>",
            Description = "Ned Stark braces for <text>",
            Example = "Brace yourself users")]
        [RespondByRegex("brace yourself (.+)")]
        public void BraceYourself(IMessage message, IHostAdapter host, string[] matches)
        {
            MemGen(message, host, "http://i.imgur.com/cOnPlV7.jpg", "Brace Yourself", matches[1]);
        }

        [Help(Syntax = "All your <text> are belong to <text>",
            Description = "All your <text> are belong to <text>",
            Example = "All your bases are belong to us")]
        [RespondByRegex("(All your .*) (are belong to .*)")]
        public void AllYourBelongTo(IMessage message, IHostAdapter host, string[] matches)
        {
            MemGen(message, host, "http://i.imgur.com/gzPiQ8R.jpg", matches[1], matches[2]);
        }

        [Help(Syntax = "<text> ALL the <things>",
            Description = "Generates ALL THE THINGS",
            Example = "Automate ALL the Things")]
        [RespondByRegex("(.*) (ALL the .*)")]
        public void AllTheThings(IMessage message, IHostAdapter host, string[] matches)
        {
            MemGen(message, host, "http://memecaptain.com/all_the_things.jpg", matches[1], matches[2]);
        }

        [Help(Syntax = "I don't always <something> but when i do <text>",
            Description = "Generates The Most Interesting man in the World",
            Example = "I don't always debug but when i do its in production")]
        [RespondByRegex("(I DON'?T ALWAYS .*) (BUT WHEN I DO,? .*)")]
        public void DontAlways(IMessage message, IHostAdapter host, string[] matches)
        {
            MemGen(message, host, "http://memecaptain.com/most_interesting.jpg", matches[1], matches[2]);
        }

        [Help(Syntax = "Y U NO <text>",
            Description = "Generates the Y U NO GUY with the bottom caption of <text>",
            Example = "Y U NO Connex")]
        [RespondByRegex("Y U NO (.+)")]
        public void WhyYouNo(IMessage message, IHostAdapter host, string[] matches)
        {
            MemGen(message, host, "http://memecaptain.com/y_u_no.jpg", "Y U NO", matches[1]);
        }

        [Help(Syntax = "<text> (SUCCESS|NAILED IT)",
            Description = "Generates success kid with the top caption of <text>",
            Example = "Jonathan NAILED IT")]
        [RespondByRegex("(.*)(SUCCESS|NAILED IT.*)")]
        public void NailedIt(IMessage message, IHostAdapter host, string[] matches)
        {
            MemGen(message, host, "http://memecaptain.com/success_kid.jpg", matches[1], matches[2]);
        }

        private void MemGen(IMessage message, IHostAdapter host, string urlString, string text1, string text2)
        {
            try
            {
                string result = CreateHttpClient(string.Format("http://memecaptain.com/g?u={0}&t1={1}&t2={2}", urlString, text1, text2))
                    .GetAsync("")
                    .Result
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                string startOfUrl = result.Substring(result.IndexOf("http"));
                string imgUrl = startOfUrl.Substring(0, startOfUrl.IndexOf("\""));
                host.ReplyTo(message, imgUrl);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-MemeGenerator", ex.ToString());
                host.ReplyTo(message, "MemGenerator crashed!");
            }
        }
    }
}