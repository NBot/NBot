namespace NBot.Core.MessageFilters
{
    public interface IMessageFilter
    {
        bool FilterMessage(Message message);
        bool FilterMessage(ref string message);
    }
}