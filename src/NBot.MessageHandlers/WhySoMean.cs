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
                                 "http://images.hanselminutes.com/images/153.jpg",
                                 "http://th01.deviantart.net/fs70/PRE/i/2013/048/f/5/why_are_you_so_mean_batman__by_shadowssyndrom-d5vbgxt.jpg",
                                 "https://i.chzbgr.com/maxW500/5290942208/h76500D06/",
                                 "http://cdn.memegenerator.net/instances/400x/33871258.jpg",
                                 "http://keenetrial.com/blog/wp-content/uploads/2014/02/why-you-gotta-be-so-mean-300x238.jpg",
                                 "http://fc01.deviantart.net/fs50/f/2009/285/c/1/Why_U_So_Mean_2_Me__by_Drago95.jpg",
                                 "http://www.troll.me/images/sad-guy/you-are-so-mean.jpg"

                             };
        [Hear("(Why|y) (so mean)(\\?)?")]
        [Help(Syntax = "<Why|y> so mean?", Description = "Show image from scott saying why so mean?", Example = "Hey why so mean?")]
        public void HandlWhySoMean(Message message, IMessageClient client)
        {
            client.ReplyTo(message, GetRandomItem(means));
        }
    }
}