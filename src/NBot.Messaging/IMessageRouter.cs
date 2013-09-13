using System.Collections;
using NBot.Messaging.MessageFilters;

namespace NBot.Messaging
{
    public interface IMessageRouter
    {
        void RegisterMessageHandler(IMessageHandler handler);
        void RegisterAdapter(IAdapter adapter, string channel);
        void RegisterContentFilter(IMessageFilter filter);
    }
}
