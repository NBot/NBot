using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDD.NBot.Core;
using SDD.NBot.Core.Messaging;
using SDD.NBot.Core.Messaging.Attributes;

namespace SDD.NBot.Plugins
{
    public class SayHi : RecieveMessages
    {
        [RespondByRegex("Hello")]
        public void DoSayHi(IUserMessage message, IHostAdapter host)
        {
            IEntity user = host.GetUser(message.UserId);
            host.ReplyTo(message, string.Format("Hi {0}", user.Name));
        }
    }
}
