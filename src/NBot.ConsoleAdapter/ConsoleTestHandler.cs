using NBot.Core;
using NBot.Core.Attributes;

namespace NBot.ConsoleAdapter
{
    public class ConsoleTestHandler : IMessageHandler
    {
        [Hear("^echo (.*)")]
        public void HandleMessaeg(Message message, IMessageClient client, string[] matches)
        {
            client.Send(matches[1], message.RoomId);
        }
    }
}