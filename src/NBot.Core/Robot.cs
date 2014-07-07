using System;
using System.Collections.Generic;
using System.Reflection;
using NBot.Core.Brains;
using NBot.Core.Help;
using NBot.Core.Logging;
using NBot.Core.MessageFilters;
using ServiceStack;
using Topshelf;

namespace NBot.Core
{
    public class Robot : IRobotConfiguration, IRobotHost
    {
        private static readonly Dictionary<string, object> Settings = new Dictionary<string, object>();
        private readonly string _environment;
        private readonly Dictionary<string, HelpInformation> _helpers = new Dictionary<string, HelpInformation>();
        private readonly List<IMessageProducer> _producers = new List<IMessageProducer>();
        private readonly IMessageRouter _router;

        private Robot(string name, string alias, string environment = "debug")
        {
            Name = name;
            Alias = alias;
            Log = new ConsoleLog();
            _router = new MessageRouter(new SimpleBrain());
            _environment = environment;
        }

        public static string Name { get; set; }
        public static string Alias { get; set; }
        public static INBotLog Log { get; private set; }

        public IRobotConfiguration AddSetting<T>(string key, T value)
        {
            if (Settings.ContainsKey(key))
                throw new ApplicationException("There is already a setting with that key.");

            Settings.Add(key, value);

            return this;
        }

        public IRobotConfiguration UseBrain(IBrain brain)
        {
            _router.RegisterBrain(brain);
            return this;
        }

        public IRobotConfiguration RegisterAdapter(IAdapter adapter, string channel)
        {
            _router.RegisterAdapter(adapter, channel);
            _producers.Add(adapter.Producer);
            return this;
        }

        public IRobotConfiguration RegisterHandler(IMessageHandler handler, params string[] allowedRooms)
        {
            UpdateHelpInformation(handler);
            _router.RegisterMessageHandler(handler, allowedRooms);
            return this;
        }

        public IRobotConfiguration RegisterMessageFilter(IMessageFilter messageFilter)
        {
            _router.RegisterMessageFilter(messageFilter);
            return this;
        }

        public IRobotConfiguration UseLog(INBotLog log)
        {
            Log = log;
            return this;
        }

        public void Run()
        {
            RegisterHandler(new Help.Help(_helpers.Values));

            HostFactory.Run(config =>
            {
                config.Service<IRobotHost>(s =>
                {
                    s.ConstructUsing(x => this);
                    s.WhenStarted(x => x.StartHost());
                    s.WhenStopped(x => x.StopHost());
                });

                config.SetServiceName("{0}_Service".FormatWith(Name));
                config.SetInstanceName(_environment);
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

        public static T GetSetting<T>(string key)
        {
            object value;

            if (Settings.TryGetValue(key, out value))
            {
                return value is T ? (T) value : default(T);
            }

            return default(T);
        }

        public static IRobotConfiguration Create(string name, string alias, string environment = "debug")
        {
            return new Robot(name, alias, environment);
        }

        public static IRobotConfiguration Create(string name)
        {
            return Create(name, name);
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