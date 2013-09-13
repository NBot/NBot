using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public delegate void MessageProducedHandler(IMessage message);

    public interface IMessageProducer
    {
        event MessageProducedHandler MessageProduced;
        void StarProduction();
        void StopProduction();
    }
}
