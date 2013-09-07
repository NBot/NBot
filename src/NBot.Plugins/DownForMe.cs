using System;
using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;
using log4net;

namespace NBot.Plugins
{
    public class DownForMe : RecieveMessages
    {
        private const string DownForMeSite = "http://www.downforeveryoneorjustme.com/";
        private const string SiteIsUpReges = "It's just you";


        
        [RespondByRegex("is (.*) down for you\\??")]
        [Help(Syntax = "is <site> down for you?",Description = "see if a site is down from the internet.", Example = "nbot is www.google.com down for you?")]
        public void IsItDownForYou(IMessage message, IHostAdapter host, string[] matches, ILog log)
        {
            DoIsItDownForYou(message, host, matches, log);
        }

        [RespondByRegex("downforyou (.*)")]
        [Help(Syntax = "downforyou <site>", Description = "see if a site is down from the internet.", Example = "nbot downforyou www.google.com")]
        public void DownForYou(IMessage message, IHostAdapter host, string[] matches, ILog log)
        {
            DoIsItDownForYou(message, host, matches, log);
        }

        private void DoIsItDownForYou(IMessage message, IHostAdapter host, string[] matches, ILog log)
        {
            try
            {
                var client = CreateHttpClient(DownForMeSite);
                string result = client.GetAsync(matches[1]).Result.Content.ReadAsStringAsync().Result;

                var match = Regex.Match(result, SiteIsUpReges);

                host.ReplyTo(message, match.Success ? string.Format("It's just you. {0} is up for me.", matches[1]) : string.Format("Oh no {0} is down for me too!!!!! *PANIC*", matches[1]));
            }
            catch (Exception e)
            {
                host.ReplyTo(message, "Oh no I am Down!!! *UberPanic*");
                log.Error("Oh no NBot is Down!!!!", e);
            }
        }
    }
}
