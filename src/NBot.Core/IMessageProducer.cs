namespace NBot.Core
{
    public delegate void MessageProducedHandler(Message message);

    public interface IMessageProducer
    {
        string Channel { get; }
        event MessageProducedHandler MessageProduced;
        void StarProduction();
        void StopProduction();
    }
}