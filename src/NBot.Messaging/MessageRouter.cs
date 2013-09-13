using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NBot.Messaging.Attributes;
using NBot.Messaging.MessageFilters;
using NBot.Messaging.Routes;

namespace NBot.Messaging
{
    public class MessageRouter : IMessageRouter
    {
        private readonly List<IMessageProducer>  _producers = new List<IMessageProducer>();
        private readonly List<IMessageFilter> _filters = new List<IMessageFilter>();
        private readonly List<IRoute> _routes = new List<IRoute>();
        private readonly Dictionary<string, IAdapter> _adapters = new Dictionary<string, IAdapter>();

        public void RegisterContentFilter(IMessageFilter filter)
        {
            _filters.Add(filter);
        }

        public void RegisterMessageHandler(IMessageHandler handler)
        {
            var handlerType = handler.GetType();

            foreach (var endpoint in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                var messageAttributes = endpoint.GetCustomAttributes(typeof(HandleMessageAttribute), true);

                foreach (HandleMessageAttribute messageAttribute in messageAttributes)
                {
                    _routes.Add(messageAttribute.CreateRoute(handlerType, endpoint));
                }
            }
        }

        public void RegisterAdapter(IAdapter adapter, string channel)
        {
            if (_adapters.ContainsKey(channel))
                throw new ApplicationException("There is already an adapter registered on that channel.");
            
            var filteredAdapter = new MessageFilterAdapter(adapter, _filters);

            _adapters.Add(channel, filteredAdapter);

            if (filteredAdapter.Producer != null)
            {
                filteredAdapter.Producer.MessageProduced += OnMessageProduced;
            }
        }

        private void OnMessageProduced(Message message)
        {
            try
            {
                var adapter = _adapters[message.Channel];
                var client = adapter.Client;

                foreach (var route in _routes)
                {
                    if (route.IsMatch(message))
                    {
                        var handler = Activator.CreateInstance(route.Handler);
                        var endpoint = route.EndPoint;
                        endpoint.Invoke(handler, BuildParameters(endpoint, message, client, route.GetMatchMetaData(message)));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private object[] BuildParameters(MethodInfo method, Message message, IMessageClient client, string[] metaData)
        {
            ParameterInfo[] methodParameters = method.GetParameters();
            var result = new object[methodParameters.Count()];

            for (int parameterIndex = 0; parameterIndex < result.Length; parameterIndex++)
            {
                ParameterInfo parameter = methodParameters[parameterIndex];

                if (parameter.ParameterType.IsAssignableFrom(typeof(Message)))
                {
                    result[parameterIndex] = message;
                }
                else if (parameter.ParameterType == typeof(string[])
                         && (parameter.Name == "matches"
                             || parameter.Name == "metadata"))
                {
                    result[parameterIndex] = metaData;
                }
                else if (parameter.ParameterType == typeof(IMessageClient))
                {
                    result[parameterIndex] = client;
                }
                //else
                //{
                //    result[parameterIndex] = _container.Resolve(parameter.ParameterType);
                //}
            }

            return result;
        }
    }
}