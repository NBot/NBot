using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public interface IMessageBroker
    {
        void RegisterMessageHandler(IMessageHandler handler);
        void RegisterMessageProducer(IMessageProducer producer, string channel);
        void RegisterMessageClient(IMessageClient client, string channel);
    }
}
