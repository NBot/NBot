namespace NBot.Core.Messaging.ContentFilters
{
    public interface IContentFilter
    {
        bool FilterMessage(IMessage message);
    }
}