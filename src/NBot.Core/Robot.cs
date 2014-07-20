using NBot.Core.Brains;
using NBot.Core.Logging;
using NBot.Core.Routes;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topshelf;

namespace NBot.Core
{
    public class Robot : IRobotHost
    {
        private static Dictionary<string, object> _settings;
        private Dictionary<string, IAdapter> _adapters;
        private readonly List<IMessageProducer> _producers = new List<IMessageProducer>();
        private List<IRoute> _routes;

        public Robot(string name, string alias, string environment, Dictionary<string, object> settings, IBrain brain,
            INBotLog log)
        {
            Name = name;
            Alias = alias;
            Environment = environment;
            _settings = settings;
            Brain = brain;
            Log = log;
        }

        public static string Name { get; private set; }
        public static string Alias { get; private set; }
        public string Environment { get; private set; }
        public static INBotLog Log { get; private set; }
        public static IBrain Brain { get; private set; }


        private static object[] BuildParameters(MethodInfo method, Message message, IMessageClient client, Dictionary<string, string> inputParameters)
        {
            var methodParameters = method.GetParameters();
            var result = new object[methodParameters.Count()];

            for (var parameterIndex = 0; parameterIndex < result.Length; parameterIndex++)
            {
                var parameter = methodParameters[parameterIndex];

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

        public static RobotBuilder Create(string name = "nbot", string alias = "nbot", string environment = "debug")
        {
            return new RobotBuilder(name, alias, environment);
        }

        public static T GetSetting<T>(string key)
        {
            object value;

            if (_settings.TryGetValue(key, out value))
            {
                return value is T ? (T)value : default(T);
            }

            return default(T);
        }

        public void Initalize(List<IRoute> routes, Dictionary<string, IAdapter> adapters)
        {
            _routes = routes;
            _adapters = adapters;
        }

        private void OnMessageProduced(Message message)
        {
            Log.WriteInfo(string.Format("Message Produced - RoomId:{0}, UserId:{1}, Content:'{2}'", message.RoomId,
                message.UserId, message.Content));

            try
            {
                IAdapter adapter = _adapters[message.Channel];
                string[] pipeline = message.Content.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                IMessageClient client = null;
                var previousSegmentOutput = new string[] { };

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
                            inputParameters = ((IMessageParameterProvider)routeToProcess).GetInputParameters(segment);
                        }
                        else if (routeToProcess is IPipedParameterProvider)
                        {
                            inputParameters =
                                ((IPipedParameterProvider)routeToProcess).GetInputParameters(previousSegmentOutput);
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

        public void Run()
        {
            foreach (var adapter in _adapters.Values)
            {
                if (adapter.Producer != null)
                {
                    adapter.Producer.MessageProduced += OnMessageProduced;
                }
                _producers.Add(adapter.Producer);
            }

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
                config.SetInstanceName(Environment);
            });
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
    }
}