using System;
using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;
using ServiceStack.Service;

namespace NBot.MessageHandlers
{
    [Tag("Productivity")]
    public class DownForMe : MessageHandler
    {
        private const string DownForMeSite = "http://www.downforeveryoneorjustme.com/";
        private const string SiteIsUpReges = "It's just you";

        [Respond("downforyou {{site}}")]
        [Respond("is {{site}} down for you\\??")]
        [Help(Syntax = "is <site> down for you?", Description = "see if a site is down from the internet.", Example = "nbot is www.google.com down for you?")]
        [Help(Syntax = "downforyou <site>", Description = "see if a site is down from the internet.", Example = "nbot downforyou www.google.com")]
        public void DoIsItDownForYou(Message message, IMessageClient client, string site)
        {
            try
            {
                IRestClient jsonClient = GetJsonServiceClient(DownForMeSite);
                var result = jsonClient.Get<string>(site);
                Match match = Regex.Match(result, SiteIsUpReges);
                client.ReplyTo(message, match.Success ? string.Format("It's just you. {0} is up for me.", site) : string.Format("Oh no {0} is down for me too!!!!! *PANIC*", site));
            }
            catch (Exception e)
            {
                client.Send("Oh no I am Down!!! *UberPanic*", message.RoomId);
            }
        }
    }
}