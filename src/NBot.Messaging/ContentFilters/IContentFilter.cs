namespace NBot.Messaging.ContentFilters
{
    public interface IContentFilter
    {
        bool FilterMessage(Message message);
    }
}