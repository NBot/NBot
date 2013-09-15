using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Achievement : MessageHandler
    {
        [Help(Syntax = "[{0}|{1}] achievement [get|unlock|unlocked] <achievement text>", Description = "Get an XBox badge with the achievement on it.", Example = "nbot achievement unlocked Winning!")]
        [Respond("achievement (get|unlock(ed)?) (.+)")]
        public void DoAchievement(Message message, IMessageClient client, string[] matches)
        {
            string caption = UrlEncode(matches.Length == 4 ? matches[3] : matches[2]);
            string url = string.Format("http://achievement-unlocked.heroku.com/xbox/{0}.png", caption);
            client.ReplyTo(message, url);
        }
    }
}