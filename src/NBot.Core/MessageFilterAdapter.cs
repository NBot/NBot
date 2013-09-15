using System.Collections.Generic;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public class MessageFilterAdapter : IAdapter
    {
        public MessageFilterAdapter(IAdapter adapter, List<IMessageFilter> filters)
        {
            InnerAdapter = adapter;
            Producer = new MessageFilterProducer(adapter.Producer, filters);
            Client = new MessageFilterClient(adapter.Client, filters);
        }

        public IAdapter InnerAdapter { get; private set; }
        public IMessageProducer Producer { get; private set; }
        public IMessageClient Client { get; private set; }
    }
}