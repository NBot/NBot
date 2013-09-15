using System.Collections.Generic;
using System.Linq;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public class MessageFilterProducer : IMessageProducer
    {
        private readonly List<IMessageFilter> _filters;
        private readonly IMessageProducer _innerMessageProducer;

        public MessageFilterProducer(IMessageProducer innerMessageProducer, List<IMessageFilter> filters)
        {
            _innerMessageProducer = innerMessageProducer;
            _filters = filters;
            _innerMessageProducer.MessageProduced += InnerMessageProducerOnMessageProduced;
        }

        public event MessageProducedHandler MessageProduced;

        public string Channel
        {
            get { return _innerMessageProducer.Channel; }
        }

        public void StarProduction()
        {
            _innerMessageProducer.StarProduction();
        }

        public void StopProduction()
        {
            _innerMessageProducer.StopProduction();
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
    }
}