namespace NBot.Core.Messaging
{
    public interface IMessage
    {
        int RoomId { get; set; }
        string Content { get; set; }
        string MessageType { get;}

    }
}