using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Achievement : RecieveMessages
    {
        [RespondByRegex("achievement (get|unlock(ed)?) (.+)")]
        public void DoAchievement(IMessage message, IHostAdapter host, string[] matches)
        {
            var caption = Uri.EscapeUriString(matches.Length == 4 ? matches[3] : matches[2]);
            var url = string.Format("http://achievement-unlocked.heroku.com/xbox/{0}.png", caption);
            host.ReplyTo(message, url);
        }
    }
}
