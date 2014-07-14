using NBot.Core.Attributes;
using NBot.Core.Brains;
using NBot.Core.Help;
using NBot.Core.Logging;
using NBot.Core.MessageFilters;
using NBot.Core.Routes;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topshelf;

namespace NBot.Core
{
    public class Robot : IRobotConfiguration, IRobotHost
    {
        private static readonly Dictionary<string, object> Settings = new Dictionary<string, object>();
        private readonly Dictionary<string, IAdapter> _adapters = new Dictionary<string, IAdapter>();
        private readonly string _environment;
        private readonly List<IMessageFilter> _filters = new List<IMessageFilter>();
        private readonly Dictionary<string, HelpInformation> _helpers = new Dictionary<string, HelpInformation>();
        private readonly List<IMessageProducer> _producers = new List<IMessageProducer>();
        private readonly List<IRoute> _routes = new List<IRoute>();

        private Robot(string name, string alias, string environment = "debug")
        {
            Name = name;
            Alias = alias;
            Log = new ConsoleLog();
            _environment = environment;
        }

        public static string Name { get; set; }
        public static string Alias { get; set; }
        public static INBotLog Log { get; private set; }

        public static IBrain Brain { get; private set; }

        public IRobotConfiguration AddSetting<T>(string key, T value)
        {
            if (Settings.ContainsKey(key))
                throw new ApplicationException("There is already a setting with that key.");

            Settings.Add(key, value);

            return this;
        }

        public IRobotConfiguration RegisterAdapter(IAdapter adapter, string channel)
        {
            if (_adapters.ContainsKey(channel))
                throw new ApplicationException("There is already an adapter registered on that channel.");

            var filteredAdapter = new MessageFilterAdapter(adapter, _filters);

            _adapters.Add(channel, filteredAdapter);

            if (filteredAdapter.Producer != null)
            {
                filteredAdapter.Producer.MessageProduced += OnMessageProduced;
            }
            _producers.Add(adapter.Producer);
            return this;
        }

        public IRobotConfiguration RegisterMessageHandler(IMessageHandler handler, params string[] allowedRooms)
        {
            UpdateHelpInformation(handler);

            Type handlerType = handler.GetType();

            foreach (MethodInfo endpoint in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] messageAttributes = endpoint.GetCustomAttributes(typeof (HandleMessageAttribute), true);

                foreach (HandleMessageAttribute messageAttribute in messageAttributes)
                {
                    IRoute route = messageAttribute.CreateRoute(handler, endpoint);

                    _routes.Add(allowedRooms == null || !allowedRooms.Any()
                        ? route
                        : new RoomSecurityRoute(route, allowedRooms));
                }
            }

            return this;
        }

        public IRobotConfiguration RegisterMessageFilter(IMessageFilter messageFilter)
        {
            _filters.Add(messageFilter);
            return this;
        }

        public void Run()
        {
            RegisterMessageHandler(new Help.Help(_helpers.Values));

            HostFactory.Run(config =>
            {
                config.Service<IRobotHost>(s =>
                {
                    s.ConstructUsing(x => this);
                    s.WhenStarted(x =>
                    {
                        Log.WriteInfo("Starting Service..");
                        x.StartHost();
                    });
                    s.WhenStopped(x =>
                    {
                        Log.WriteInfo("Stoping Service..");
                        x.StopHost();
                    });
                });

                config.SetServiceName("{0}_Service".FormatWith(Name));
                config.SetInstanceName(_environment);
            });
        }

        public IRobotConfiguration UseBrain(IBrain brain)
        {
            Brain = brain;
            return this;
        }

        public IRobotConfiguration UseLog(INBotLog log)
        {
            Log = log;
            return this;
        }

        public void StartHost()
        {
            foreach (IMessageProducer messageProducer in _producers)
            {
                messageProducer.StarProduction();
            }
        }

        public void StopHost()
        {
            foreach (IMessageProducer messageProducer in _producers)
            {
                messageProducer.StopProduction();
            }
        }

        private object[] BuildParameters(MethodInfo method, Message message, IMessageClient client,
            Dictionary<string, string> inputParameters)
        {
            ParameterInfo[] methodParameters = method.GetParameters();
            var result = new object[methodParameters.Count()];

            for (int parameterIndex = 0; parameterIndex < result.Length; parameterIndex++)
            {
                ParameterInfo parameter = methodParameters[parameterIndex];

                if (parameter.ParameterType.IsAssignableFrom(typeof (Message)))
                {
                    result[parameterIndex] = message;
                }
                else if (parameter.ParameterType == typeof (IMessageClient))
                {
                    result[parameterIndex] = client;
                }
                else if (parameter.ParameterType == typeof (IBrain))
                {
                    result[parameterIndex] = Brain;
                }
                else
                {
                    string value;
                    if (inputParameters.TryGetValue(parameter.Name, out value))
                    {
                        result[parameterIndex] = Convert.ChangeType(value, parameter.ParameterType);
                    }
                }
            }

            return result;
        }

        public static IRobotConfiguration Create(string name)
        {
            return Create(name, name);
        }

        public static IRobotConfiguration Create(string name, string alias, string environment = "debug")
        {
            return new Robot(name, alias, environment);
        }

        public static T GetSetting<T>(string key)
        {
            object value;

            if (Settings.TryGetValue(key, out value))
            {
                return value is T ? (T) value : default(T);
            }

            return default(T);
        }

        private void OnMessageProduced(Message message)
        {
            Log.WriteInfo(string.Format("Message Produced - RoomId:{0}, UserId:{1}, Content:'{2}'", message.RoomId,
                message.UserId, message.Content));

            try
            {
                IAdapter adapter = _adapters[message.Channel];
                string[] pipeline = message.Content.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

                IMessageClient client = null;
                var previousSegmentOutput = new string[] {};

                for (int segmentIndex = 0; segmentIndex < pipeline.Length; segmentIndex++)
                {
                    Message segment = message.CloneWithNewContent(pipeline[segmentIndex]);
                    var pipedMessageClient = client as PipedMessageClient;

                    // Replace the input to the next command with the output of the previous
                    if (pipedMessageClient != null)
                    {
                        string content = pipedMessageClient.ReplaceInput(segment.Content.Trim());
                        previousSegmentOutput = pipedMessageClient.Output;
                        segment = segment.CloneWithNewContent(content);
                    }

                    client = segmentIndex == pipeline.Length - 1
                        ? adapter.Client
                        : new PipedMessageClient(adapter.Client);


                    foreach (IRoute route in _routes.Where(r => r.IsMatch(segment)))
                    {
                        Log.WriteInfo(string.Format(
                            "Route Handled - RoomId:{0}, UserId:{1}, Content:'{2}', Route:'{3}", message.RoomId,
                            message.UserId, message.Content, route.EndPoint.Name));

                        var inputParameters = new Dictionary<string, string>();

                        IRoute routeToProcess = route;

                        var roomSecurityRoute = route as IRoomSecurityRoute;

                        if (roomSecurityRoute != null)
                        {
                            routeToProcess = roomSecurityRoute.InnerRoute;
                        }

                        if (routeToProcess is IMessageParameterProvider)
                        {
                            inputParameters = ((IMessageParameterProvider) routeToProcess).GetInputParameters(segment);
                        }
                        else if (routeToProcess is IPipedParameterProvider)
                        {
                            inputParameters =
                                ((IPipedParameterProvider) routeToProcess).GetInputParameters(previousSegmentOutput);
                        }

                        object[] routeParameters = BuildParameters(route.EndPoint, segment, client, inputParameters);
                        routeToProcess.EndPoint.Invoke(routeToProcess.Handler, routeParameters);
                    }
                }
            }
            catch (Exception e)
            {
                Log.WriteError(
                    string.Format("An error has occurred while processing message \"{0}\"", message.Content), e);
            }
        }

        private void UpdateHelpInformation(IMessageHandler handler)
        {
            Type type = handler.GetType();

            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                object[] helpAttributes = methodInfo.GetCustomAttributes(typeof (HelpAttribute), true);

                if (!_helpers.ContainsKey(type.Name))
                {
                    var helper = new HelpInformation
                    {
                        Plugin = type.Name,
                        Commands = new List<Command>()
                    };

                    _helpers.Add(type.Name, helper);
                }

                foreach (HelpAttribute helpAttribute in helpAttributes)
                {
                    _helpers[type.Name].Commands.Add(new Command(helpAttribute.Syntax.FormatWith(Name, Alias),
                        helpAttribute.Description.FormatWith(Name, Alias), helpAttribute.Example.FormatWith(Name, Alias)));
                }
            }
        }
    }
}