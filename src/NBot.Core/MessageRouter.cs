using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NBot.Core.Attributes;
using NBot.Core.Brains;
using NBot.Core.Logging;
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

        public void RegisterMessageHandler(IMessageHandler handler, List<string> allowedRooms = null)
        {
            Type handlerType = handler.GetType();

            foreach (MethodInfo endpoint in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                var messageAttributes = endpoint.GetCustomAttributes(typeof(HandleMessageAttribute), true);

                foreach (HandleMessageAttribute messageAttribute in messageAttributes)
                {
                    var route = messageAttribute.CreateRoute(handler, endpoint);

                    _routes.Add(allowedRooms == null ? route : new RoomSecurityRoute(route, allowedRooms));
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
                var pipeline = message.Content.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                IMessageClient client = null;
                string[] previousSegmentOutput = new string[] { };

                for (int segmentIndex = 0; segmentIndex < pipeline.Length; segmentIndex++)
                {
                    var segment = message.CloneWithNewContent(pipeline[segmentIndex]);
                    var pipedMessageClient = client as PipedMessageClient;

                    // Replace the input to the next command with the output of the previous
                    if (pipedMessageClient != null)
                    {
                        var content = pipedMessageClient.ReplaceInput(segment.Content.Trim());
                        previousSegmentOutput = pipedMessageClient.Output;
                        segment = segment.CloneWithNewContent(content);
                    }

                    client = segmentIndex == pipeline.Length - 1 ? adapter.Client : new PipedMessageClient(adapter.Client);


                    foreach (var route in _routes.Where(r => r.IsMatch(segment)))
                    {
                        Dictionary<string, string> inputParameters = new Dictionary<string, string>();

                        if (route is IMessageParameterProvider)
                        {
                            inputParameters = ((IMessageParameterProvider)route).GetInputParameters(segment);
                        }
                        else if (route is IPipedParameterProvider)
                        {
                            inputParameters = ((IPipedParameterProvider)route).GetInputParameters(previousSegmentOutput);
                        }

                        var routeParameters = BuildParameters(route.EndPoint, segment, client, inputParameters);
                        route.EndPoint.Invoke(route.Handler, routeParameters);
                    }

                }
            }
            catch (Exception e)
            {
                Robot.Log.WriteError(string.Format("An error has occurred while processing message \"{0}\"", message.Content), e);
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