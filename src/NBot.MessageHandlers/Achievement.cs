using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    [Tag("Fun", "Recognition")]
    public class Achievement : MessageHandler
    {
        [Help(Syntax = "[{0}|{1}] achievement [get|unlock|unlocked] <achievement text>", Description = "Get an XBox badge with the achievement on it.", Example = "nbot achievement unlocked Winning!")]
        [Respond("achievement (get|unlock(ed)?) {{caption}}")]
        public void DoAchievement(Message message, IMessageClient client, string caption)
        {
            string encodedCaption = UrlEncode(caption);
            string url = string.Format("http://achievement-unlocked.heroku.com/xbox/{0}.png", encodedCaption);
            client.ReplyTo(message, url);
        }
    }
}