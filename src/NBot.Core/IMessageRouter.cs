using System.Collections.Generic;
using NBot.Core.Brains;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public interface IMessageRouter
    {
        void RegisterMessageHandler(IMessageHandler handler, List<string> allowedRooms);
        void RegisterAdapter(IAdapter adapter, string channel);
        void RegisterMessageFilter(IMessageFilter filter);
        void RegisterBrain(IBrain brain);
    }
}