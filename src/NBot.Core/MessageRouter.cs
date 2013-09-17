using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NBot.Core.Attributes;
using NBot.Core.Brains;
using NBot.Core.MessageFilters;
using NBot.Core.Routes;
using ServiceStack.Validation;

namespace NBot.Core
{
    public class MessageRouter : IMessageRouter
    {
        private IBrain _brain;
        private readonly Dictionary<string, IAdapter> _adapters = new Dictionary<string, IAdapter>();
        private readonly List<IMessageFilter> _filters = new List<IMessageFilter>();
        private readonly List<IRoute> _routes = new List<IRoute>();

        public MessageRouter(IBrain brain)
        {
            _brain = brain;
        }

        public void RegisterMessageFilter(IMessageFilter filter)
        {
            _filters.Add(filter);
        }

        public void RegisterBrain(IBrain brain)
        {
            _brain = brain;
        }

        public void RegisterMessageHandler(IMessageHandler handler)
        {
            Type handlerType = handler.GetType();

            foreach (MethodInfo endpoint in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] messageAttributes = endpoint.GetCustomAttributes(typeof(HandleMessageAttribute), true);

                foreach (HandleMessageAttribute messageAttribute in messageAttributes)
                {
                    _routes.Add(messageAttribute.CreateRoute(handler, endpoint));
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
                IAdapter adapter = _adapters[message.Channel];
                IMessageClient innerClient = adapter.Client;

                var pipeSegment = message.Content.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                IMessageClient client = null;

                for (int pipeSegmentIndex = 0; pipeSegmentIndex < pipeSegment.Length; pipeSegmentIndex++)
                {
                    var pipeMessage = message.CloneWithNewContent(pipeSegment[pipeSegmentIndex]);
                    var pipedMessageClient = client as PipedMessageClient;

                    // Replace the input to the next command with the output of the previous
                    if (pipedMessageClient != null)
                    {
                        var content = pipedMessageClient.ReplaceInput(pipeMessage.Content.Trim());
                        pipeMessage = pipeMessage.CloneWithNewContent(content);
                    }

                    client = pipeSegmentIndex == pipeSegment.Length - 1 ? innerClient : new PipedMessageClient(innerClient);

                    foreach (IRoute route in _routes)
                    {
                        if (route.IsMatch(pipeMessage))
                        {
                            MethodInfo endpoint = route.EndPoint;
                            endpoint.Invoke(route.Handler, BuildParameters(endpoint, pipeMessage, client, route.GetInputParameters(pipeMessage)));
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private object[] BuildParameters(MethodInfo method, Message message, IMessageClient client, Dictionary<string, string> inputParameters)
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
                else if (parameter.ParameterType == typeof(IMessageClient))
                {
                    result[parameterIndex] = client;
                }
                else if (parameter.ParameterType == typeof(IBrain))
                {
                    result[parameterIndex] = _brain;
                }
                else if (inputParameters.ContainsKey(parameter.Name))
                {
                    result[parameterIndex] = Convert.ChangeType(inputParameters[parameter.Name], parameter.ParameterType);
                }
            }

            return result;
        }
    }
}