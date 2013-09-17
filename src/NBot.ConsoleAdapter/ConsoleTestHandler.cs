using NBot.Core;
using NBot.Core.Attributes;

namespace NBot.ConsoleAdapter
{
    public class ConsoleTestHandler : IMessageHandler
    {
        [Hear("^echo {{input}}")]
        public void HandleMessaeg(Message message, IMessageClient client, string input)
        {
            client.Send(input, message.RoomId);
        }
    }
}