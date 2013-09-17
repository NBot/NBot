using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Brains;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Remember : MessageHandler
    {
        [Respond("remember( me)? {{value}} as {{key}}")]
        [Help(Syntax = "nbot remember (me) <value> as <key>", Description = "Stores a string in the brain.", Example = "nbot remember me Jonathan as Name")]
        public void RememberMeSomeStuff(Message message, IMessageClient client, IBrain brain, string value, string key)
        {
            brain.SetValue(key, value);

            client.ReplyTo(message, string.Format("{0} stored as {1}", value, key));
        }

        [Respond("recall( me)? {{key}}")]
        [Help(Syntax = "nbot recall (me) <key>", Description = "Finds a string value in the brain.", Example = "nbot recall me Name")]
        public void RecallMeSomeStuff(Message message, IMessageClient client, IBrain brain, string key)
        {
            object value = brain.GetValue(key);
            client.ReplyTo(message, value == null ? string.Format("The key {0} is not in the brain.", key) : string.Format("The value of {0} is {1}", key, value));
        }
    }
}