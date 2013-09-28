using System;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Brains;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    [Tag("Productivity")]
    public class Status : MessageHandler
    {
        [Help(Syntax = "check me in",
            Description = "Alerts the nBot and others you have arrived",
            Example = "check me in")]
        [Respond("check( me)? {{query}}")]
        public void HandleStatusChange(Message message, IMessageClient client, IBrain brain, string query)
        {
            string time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            //string result = "wut";
            var userName = client.GetUser(message.UserId).Name;
            if (query.ToLower().StartsWith("in"))
            {
                //result = "i recognize " + message.UserId + " checked in at " + time;
                brain.SetValue(LastCheckinKey(userName), time);
                brain.RemoveKey(LastCheckoutKey(userName));
            }
            else if (query.ToLower().StartsWith("out"))
            {
                //result = "i recognize " + message.UserId + " checked out at " + time;
                brain.SetValue(LastCheckoutKey(userName), time);
                brain.RemoveKey(LastCheckinKey(userName));
            }
            //client.ReplyTo(message, result);
        }

        [Help(Syntax = "{{userName}} status",
            Description = "Tells you the status of user",
            Example = "Hillary Clinton status")]
        [Respond("{{userName}} status")]
        public void HandleGetStatus(Message message, IMessageClient client, IBrain brain, string userName)
        {
            string result = userName;
            bool needsAnd = false;
            bool neverSeenThem = true;

            if (brain.ContainsKey(LastCheckinKey(userName)))
            {
                result += " last checked in at " + brain.GetValue(LastCheckinKey(userName));
                neverSeenThem = false;
                needsAnd = true;
            }

            if (brain.ContainsKey(LastCheckoutKey(userName)))
            {
                if (needsAnd)
                {
                    result += ",";
                }
                result += " last checked out at " + brain.GetValue(LastCheckoutKey(userName));
                neverSeenThem = false;
                needsAnd = true;
            }

            if (brain.ContainsKey(LastSpokeKey(userName)))
            {
                if (needsAnd)
                {
                    result += " and";
                }
                result += " last spoke at " + brain.GetValue(LastSpokeKey(userName));
                neverSeenThem = false;
            }

            if (neverSeenThem)
            {
                result = "I ain't never seen " + userName + " come round these parts";
            }

            client.ReplyTo(message, result);
        }

        [Hear(".*")]
        public void Hear(Message message, IMessageClient client, IBrain brain)
        {
            string time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            string result = time + " in the room " + message.RoomId;
            var userName = client.GetUser(message.UserId).Name;

            brain.SetValue(LastSpokeKey(userName), result);
        }


        private string LastCheckinKey(string userName)
        {
            return userName + "LastCheckIn";
        }

        private string LastCheckoutKey(string userName)
        {
            return userName + "LastCheckOut";
        }

        private string LastSpokeKey(string userName)
        {
            return userName + "LastSpoke";
        }
    }
}