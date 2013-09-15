using System;
using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;

namespace NBot.MessageHandlers
{
    public class DownForMe : MessageHandler
    {
        private const string DownForMeSite = "http://www.downforeveryoneorjustme.com/";
        private const string SiteIsUpReges = "It's just you";


        [Respond("is (.*) down for you\\??")]
        [Help(Syntax = "is <site> down for you?", Description = "see if a site is down from the internet.", Example = "nbot is www.google.com down for you?")]
        public void IsItDownForYou(Message message, IMessageClient client, string[] matches)
        {
            DoIsItDownForYou(message, client, matches);
        }

        [Respond("downforyou (.*)")]
        [Help(Syntax = "downforyou <site>", Description = "see if a site is down from the internet.", Example = "nbot downforyou www.google.com")]
        public void DownForYou(Message message, IMessageClient client, string[] matches)
        {
            DoIsItDownForYou(message, client, matches);
        }

        private void DoIsItDownForYou(Message message, IMessageClient client, string[] matches)
        {
            try
            {
                IRestClient jsonClient = GetJsonServiceClient(DownForMeSite);
                var result = jsonClient.Get<string>(matches[1]);
                Match match = Regex.Match(result, SiteIsUpReges);
                client.ReplyTo(message, match.Success ? string.Format("It's just you. {0} is up for me.", matches[1]) : string.Format("Oh no {0} is down for me too!!!!! *PANIC*", matches[1]));
            }
            catch (Exception e)
            {
                client.Send("Oh no I am Down!!! *UberPanic*", message.RoomId);
            }
        }
    }
}