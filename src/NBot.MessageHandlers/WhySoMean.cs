using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    [Tag("Fun")]
    public class WhySoMean : MessageHandler
    {
        string[] means = new string[]
                             {
                                 "http://images.hanselminutes.com/images/153.jpg"
                             };
        [Hear("(Why|y) (so mean)(\\?)?")]
        [Help(Syntax = "<Why|y> so mean?", Description = "Show image from scott saying why so mean?", Example = "Hey why so mean?")]
        public void HandlWhySoMean(Message message, IMessageClient client)
        {
            client.ReplyTo(message, GetRandomItem(means));
        }
    }
}