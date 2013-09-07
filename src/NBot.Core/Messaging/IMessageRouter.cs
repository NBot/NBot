using System.Collections.Generic;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging
{
    public interface IMessageRouter
    {
        void AddRoute(IRoute route);
        IEnumerable<IRoute> GetRoutes(IMessage message);
        void BuildRoutes(IEnumerable<IRecieveMessages> messageRecievers);
    }
}