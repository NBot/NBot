using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using NBot.Core.Attributes;
using NBot.Core.Brains;
using NBot.Core.Help;
using NBot.Core.Logging;
using NBot.Core.MessageFilters;
using NBot.Core.Routes;
using ServiceStack;

namespace NBot.Core
{
    public class RobotBuilder
    {
        private readonly RobotConfiguration _configuration;
        private readonly List<MessageHandlerRegistration> _messageHandlerRegistrations = new List<MessageHandlerRegistration>();
        private readonly List<Func<RobotConfiguration, IMessageFilter>> _messagFilterRegistrations = new List<Func<RobotConfiguration, IMessageFilter>>();
        private readonly Dictionary<string, Func<RobotConfiguration, IAdapter>> _adapterRegistrations = new Dictionary<string, Func<RobotConfiguration, IAdapter>>();
        private readonly Dictionary<string, HelpInformation> _helpers = new Dictionary<string, HelpInformation>();

        public RobotBuilder(string name = "nbot", string alias = "nbot", string environment = "debug")
        {
            _configuration = new RobotConfiguration(name, alias, environment);
        }


        public RobotBuilder AddSetting<T>(string key, T value)
        {
            _configuration.Settings[key] = value;
            return this;
        }

        public RobotBuilder UseBrain(IBrain brain)
        {
            _configuration.Brain = brain;
            return this;
        }

        public RobotBuilder UseLog(INBotLog log)
        {
            _configuration.Log = log;
            return this;
        }

        public RobotBuilder RegisterAdapter(string channel, IAdapter adapter)
        {
            _adapterRegistrations[channel] = configuration => adapter;
            return this;
        }

        public RobotBuilder RegisterAdapter(string channel, Func<RobotConfiguration, IAdapter> adapterRegistration)
        {
            _adapterRegistrations[channel] = adapterRegistration;
            return this;
        }

        public RobotBuilder RegisterMessageHandler(IMessageHandler messageHandler, params string[] allowedRooms)
        {
            return RegisterMessageHandler(configuration => messageHandler, allowedRooms);
        }

        public RobotBuilder RegisterMessageHandler(Func<RobotConfiguration, IMessageHandler> registrationFunction, params string[] allowedRooms)
        {
            _messageHandlerRegistrations.Add(new MessageHandlerRegistration(registrationFunction, allowedRooms));
            return this;
        }

        public RobotBuilder RegisterMessageFilter(IMessageFilter messageFilter)
        {
            return RegisterMessageFilter(configuration => messageFilter);
        }

        public RobotBuilder RegisterMessageFilter(Func<RobotConfiguration, IMessageFilter> registration)
        {
            _messagFilterRegistrations.Add(registration);
            return this;
        }

        public Robot Build()
        {
            LoadSettings();
            
            var result = new Robot(_configuration.Name, 
                _configuration.Alias, 
                _configuration.Environment, 
                _configuration.Settings, 
                _configuration.Brain, 
                _configuration.Log);


            var filters = BuildFilters();
            var routes = BuildRoutes().Union(BuildRoute(new Help.Help(_helpers.Values), null)).ToList();
            var adapters = BuildAdapters(filters.ToList());
            
            result.Initalize(routes, adapters);

            return result;
        }

        public void Run()
        {
            Build().Run();
        }

        private IEnumerable<IMessageFilter> BuildFilters()
        {
            return _messagFilterRegistrations.Select(messagFilterRegistration => messagFilterRegistration(_configuration));
        }

        private Dictionary<string, IAdapter> BuildAdapters(List<IMessageFilter> filters)
        {
            var result = new Dictionary<string, IAdapter>();

            foreach (var channel in _adapterRegistrations.Keys)
            {
                var adapter = _adapterRegistrations[channel](_configuration);

                if (result.ContainsKey(channel))
                    throw new ApplicationException("There is already an adapter registered on that channel.");

                var filteredAdapter = new MessageFilterAdapter(adapter, filters);

                result.Add(channel, filteredAdapter);
            }

            return result;
        }

        private IEnumerable<IRoute> BuildRoutes()
        {
            return from messageHandlerRegistration in _messageHandlerRegistrations
                   let handler = messageHandlerRegistration.RegistrationFunction(_configuration)
                   let allowedRooms = messageHandlerRegistration.AllowedRooms
                   from route in BuildRoute(handler, allowedRooms)
                   select route;
        }

        private IEnumerable<IRoute> BuildRoute(IMessageHandler handler, string[] allowedRooms)
        {
            UpdateHelpInformation(handler);

            Type handlerType = handler.GetType();

            foreach (MethodInfo endpoint in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                object[] messageAttributes = endpoint.GetCustomAttributes(typeof(HandleMessageAttribute), true);

                foreach (HandleMessageAttribute messageAttribute in messageAttributes)
                {
                    IRoute route = messageAttribute.CreateRoute(handler, endpoint);

                    yield return allowedRooms == null || !allowedRooms.Any()
                        ? route
                        : new RoomSecurityRoute(route, allowedRooms);
                }
            }
        }

        private void LoadSettings()
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                var keyTaxonomy = key.Split(new[] { ':', '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (keyTaxonomy.Length == 2 && keyTaxonomy[0].ToLower() == "nbot")
                {
                    AddSetting(keyTaxonomy[1], ConfigurationManager.AppSettings[key]);
                }
            }
        }

        private void UpdateHelpInformation(IMessageHandler handler)
        {
            Type type = handler.GetType();

            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                object[] helpAttributes = methodInfo.GetCustomAttributes(typeof(HelpAttribute), true);

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
                    _helpers[type.Name].Commands.Add(new Command(helpAttribute.Syntax.FormatWith(_configuration.Name, _configuration.Alias),
                        helpAttribute.Description.FormatWith(_configuration.Name, _configuration.Alias), helpAttribute.Example.FormatWith(_configuration.Name, _configuration.Alias)));
                }
            }
        }

    }
}