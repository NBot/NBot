namespace NBot.Core.Messaging
{
    public interface IUserMessage : IMessage
    {
        int UserId { get; set; }
    }
}