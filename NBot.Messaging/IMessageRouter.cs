using System;
using System.Collections;
using System.Collections.Generic;

namespace NBot.Messaging
{
    public interface IMessageRouter
    {
        void RegisterMessageHandler(IMessageHandler handler);
        void RegisterAdapter(IAdapter adapter, string channel);
    }

    public class MessageRouter : IMessageRouter
    {
        private Dictionary<string, IAdapter> _adapters = new Dictionary<string, IAdapter>();

        public void RegisterMessageHandler(IMessageHandler handler)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterAdapter(IAdapter adapter, string channel)
        {
            if (_adapters.ContainsKey(channel))
                throw new ApplicationException("There is already an adapter registered on that channel.");

            _adapters.Add(channel, adapter);
           
            if (adapter.Producer != null)
            {
                adapter.Producer.MessageProduced += OnMessageProduced;
            }
        }

        private void OnMessageProduced(Message message)
        {
            var adapter = _adapters[message.Channel];
            var client = adapter.Client;



        }
    }
}
