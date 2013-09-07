using NBot.Core;
using NBot.Core.Brains;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Remember : RecieveMessages
    {
        [RespondByRegex("remember( me)? (.*) as (.*)")]
        [Help(Syntax = "nbot remember (me) <value> as <key>", Description = "Stores a string in the brain.", Example = "nbot remember me Jonathan as Name")]
        public void RememberMeSomeStuff(IMessage message, IHostAdapter host, IBrain brain, string[] matches)
        {
            string value;
            string key;

            if (matches.Length == 4)
            {
                value = matches[2];
                key = matches[3];
            }
            else
            {
                value = matches[1];
                key = matches[2];
            }

            brain.SetValue(key, value);

            host.ReplyTo(message, string.Format("{0} stored as {1}", value, key));
        }

        [RespondByRegex("recall( me)? (.*)")]
        [Help(Syntax = "nbot recall (me) <key>", Description = "Finds a string value in the brain.", Example = "nbot recall me Name")]
        public void RecallMeSomeStuff(IMessage message, IHostAdapter host, IBrain brain, string[] matches)
        {
            string key = matches.Length == 3 ? matches[2] : matches[1];
            object value = brain.GetValue(key);
            host.ReplyTo(message, value == null ? string.Format("The key {0} is not in the brain.", key) : string.Format("The value of {0} is {1}", key, value));
        }
    }
}