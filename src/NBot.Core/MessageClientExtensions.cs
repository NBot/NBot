namespace NBot.Core
{
    public static class MessageClientExtensions
    {
        public static void ReplyTo(this IMessageClient client, Message message, string response)
        {
            client.Send(response, message.RoomId);
        }
    }
}
