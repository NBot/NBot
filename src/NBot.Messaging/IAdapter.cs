using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NBot.Messaging
{
    public interface IAdapter
    {
        IMessageProducer Producer { get; }
        IMessageClient Client { get; }
    }
}
