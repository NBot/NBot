using System.Collections.Generic;
using System.Linq;
using NBot.Core.Messaging.ContentFilters;

namespace NBot.Core.Messaging
{
    public class FilteringMessagingService : IMessagingService
    {
        private readonly IEnumerable<IContentFilter> _filters;
        private readonly IMessagingService _inner;

        public FilteringMessagingService(IMessagingService inner, IEnumerable<IContentFilter> filters)
        {
            _inner = inner;
            _filters = filters;
        }

        #region IMessagingService Members

        public void Publish(IMessage message)
        {
            if (!_filters.Any() || !_filters.All(f => f.FilterMessage(message)))
            {
                _inner.Publish(message);
            }
            else
            {
                message.Content = "Content was filtered.....";
                _inner.Publish(message);
            }
        }

        public TResult Send<TResult>(IMessage message)
        {
            if (!_filters.Any() || !_filters.All(f => f.FilterMessage(message)))
            {
                return _inner.Send<TResult>(message);
            }

            message.Content = "Content was filtered.....";
            return _inner.Send<TResult>(message);
        }

        #endregion
    }
}