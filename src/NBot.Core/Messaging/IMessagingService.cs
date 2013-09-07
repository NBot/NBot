namespace NBot.Core.Messaging
{
    public interface IMessagingService
    {
        void Publish(IMessage message);
        TResult Send<TResult>(IMessage message);
    }
}