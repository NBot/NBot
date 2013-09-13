using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Messaging.MessageFilters;

namespace NBot.Messaging
{
    public class MessageFilterProducer : IMessageProducer
    {
        private readonly IMessageProducer _innerMessageProducer;
        private readonly List<IMessageFilter> _filters;

        public MessageFilterProducer(IMessageProducer innerMessageProducer, List<IMessageFilter> filters)
        {
            _innerMessageProducer = innerMessageProducer;
            _filters = filters;
            _innerMessageProducer.MessageProduced += InnerMessageProducerOnMessageProduced;
        }

        private void InnerMessageProducerOnMessageProduced(Message message)
        {
            if (MessageProduced != null)
            {
                if (_filters.Any(filter => filter.FilterMessage(message)))
                {
                    return;
                }

                MessageProduced(message);
            }
        }

        public event MessageProducedHandler MessageProduced;
        
        public void StarProduction()
        {
           _innerMessageProducer.StarProduction();
        }

        public void StopProduction()
        {
            _innerMessageProducer.StopProduction();
        }
    }
}
