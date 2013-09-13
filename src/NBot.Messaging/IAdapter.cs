using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public interface IAdapter
    {
        IMessageProducer Producer { get; }
        IMessageClient Client { get; }
    }
}
