using System.Collections.Generic;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging
{
    public interface IMessageRouter
    {
        void AddRoute(IRoute route);
        IEnumerable<IRoute> GetRoutes(IMessage message);
        void BuildHandlerRoutes(IEnumerable<IHandleMessages> messageHandlers);
        void BuildRecieverRoutes(IEnumerable<IRecieveMessages> messageRecievers);
    }
}