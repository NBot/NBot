namespace NBot.Core
{
    public interface IAdapter
    {
        IMessageProducer Producer { get; }
        IMessageClient Client { get; }
    }
}