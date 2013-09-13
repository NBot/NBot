namespace NBot.Messaging.MessageFilters
{
    public interface IMessageFilter
    {
        bool FilterMessage(Message message);
    }
}