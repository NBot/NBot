using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Echo : MessageHandler
    {
        [Hear("^echo {{text}}")]
        [Help(Syntax = "echo something", Description = "echo text back.", Example = "echo WINNING")]
        public void HandleEcho(Message message, IMessageClient client, string text)
        {
            client.ReplyTo(message, text);
        }
    }
}
