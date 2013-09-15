using NBot.Core.Brains;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public interface IMessageRouter
    {
        void RegisterMessageHandler(IMessageHandler handler);
        void RegisterAdapter(IAdapter adapter, string channel);
        void RegisterMessageFilter(IMessageFilter filter);
        void RegisterBrain(IBrain brain);
    }
}