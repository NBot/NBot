using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NBot.Messaging.Attributes;
using NBot.Messaging.Routes;

namespace NBot.Messaging
{
    public interface IMessageRouter
    {
        void RegisterMessageHandler(IMessageHandler handler);
        void RegisterAdapter(IAdapter adapter, string channel);
    }

    public class MessageRouter : IMessageRouter
    {
        private readonly List<IRoute> _routes = new List<IRoute>();
        private readonly Dictionary<string, IAdapter> _adapters = new Dictionary<string, IAdapter>();

        public void RegisterMessageHandler(IMessageHandler handler)
        {
            var handlerType = handler.GetType();

            foreach (var endpoint in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] messageAttributes = endpoint.GetCustomAttributes(typeof(HandleMessageAttribute), true);

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

            _adapters.Add(channel, adapter);

            if (adapter.Producer != null)
            {
                adapter.Producer.MessageProduced += OnMessageProduced;
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
