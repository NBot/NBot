namespace NBot.Core.Messaging
{
    public interface IMessageHandler<in TMessage, out TResult>
    {
        TResult HandleMessage(TMessage message);
    }
}