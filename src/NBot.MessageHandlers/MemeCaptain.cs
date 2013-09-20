using System;
using System.Diagnostics;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class MemeCaptain : MessageHandler
    {
        [Help(Syntax = "Brace yourself <text>",
            Description = "Ned Stark braces for <text>",
            Example = "Brace yourself users")]
        [Respond("brace yourself {{caption}}")]
        public void BraceYourself(Message message, IMessageClient client, string caption)
        {
            MemeGen(message, client, "http://i.imgur.com/cOnPlV7.jpg", "Brace Yourself", caption);
        }

        [Help(Syntax = "All your <text> are belong to <text>",
            Description = "All your <text> are belong to <text>",
            Example = "All your bases are belong to us")]
        [Respond("(All your {{thing}}) (are belong to {{someone}})")]
        public void AllYourBelongTo(Message message, IMessageClient client, string thing, string someone)
        {
            MemeGen(message, client, "http://i.imgur.com/gzPiQ8R.jpg", string.Format("All your {0}", thing), string.Format("are belong to {0}", someone));
        }

        [Help(Syntax = "<text> ALL the <things>",
            Description = "Generates ALL THE THINGS",
            Example = "Automate ALL the Things")]
        [Respond("{{action}} (ALL the {{things}})")]
        public void AllTheThings(Message message, IMessageClient client, string action, string things)
        {
            MemeGen(message, client, "http://memecaptain.com/all_the_things.jpg", action, string.Format("ALL the {0}", things));
        }

        [Help(Syntax = "I don't always <something> but when i do <text>",
            Description = "Generates The Most Interesting man in the World",
            Example = "I don't always debug but when i do its in production")]
        [Respond("(I DON'?T ALWAYS {{something}}) (BUT WHEN I DO,? {{how}})")]
        public void DontAlways(Message message, IMessageClient client, string something, string how)
        {
            MemeGen(message, client, "http://memecaptain.com/most_interesting.jpg", string.Format("I DON'?T ALWAYS {0}", something), string.Format("BUT WHEN I DO,? {0}", how));
        }

        [Help(Syntax = "Y U NO <text>",
            Description = "Generates the Y U NO GUY with the bottom caption of <text>",
            Example = "Y U NO Connex")]
        [Respond("Y U NO {{doSomething}}")]
        public void WhyYouNo(Message message, IMessageClient client, string doSomething)
        {
            MemeGen(message, client, "http://memecaptain.com/y_u_no.jpg", "Y U NO", doSomething);
        }

        [Help(Syntax = "<text> (NAILED IT)",
            Description = "Generates success kid with the top caption of <text>",
            Example = "Jonathan NAILED IT")]
        [Respond("{{topCaption}} (NAILED IT)")]
        public void NailedIt(Message message, IMessageClient client, string topCaption)
        {
            MemeGen(message, client, "http://memecaptain.com/success_kid.jpg", topCaption, "NAILED IT");
        }

        [Help(Syntax = "<text> (SUCCESS|NAILED IT)",
    Description = "Generates success kid with the top caption of <text>",
    Example = "Jonathan Success")]
        [Respond("{{topCaption}} (SUCCESS)")]
        public void Success(Message message, IMessageClient client, string topCaption)
        {
            MemeGen(message, client, "http://memecaptain.com/success_kid.jpg", topCaption, "SUCCESS");
        }

        private void MemeGen(Message message, IMessageClient client, string urlString, string text1, string text2)
        {
            try
            {
                var result = GetJsonServiceClient(string.Format("http://memecaptain.com/g?u={0}&t1={1}&t2={2}", UrlEncode(urlString), UrlEncode(text1), UrlEncode(text2)))
                    .Get<string>("");

                string startOfUrl = result.Substring(result.IndexOf("http"));
                string imgUrl = startOfUrl.Substring(0, startOfUrl.IndexOf("\""));
                client.ReplyTo(message, imgUrl);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-MemeCaptain", ex.ToString());
                client.ReplyTo(message, "MemGenerator crashed!");
            }
        }
    }
}