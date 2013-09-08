using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using NBot.Core.Help;
using NBot.Core.Messaging;
using log4net;

namespace NBot.Core
{
    public class NBotHost
    {
        private readonly IComponentContext _container;
        private readonly ILog _log;
        private List<IMessageAdapter> _messageAdapters;
        private List<IMessageFeed> _messageFeeds;

        public NBotHost(IComponentContext container, ILog log)
        {
            _container = container;
            _log = log;
            InitalizeHost();
        }

        public IMessageFeed Adapter { get; private set; }

        public void StartHost()
        {
            _log.Info("Starting Host...");
            foreach (IMessageFeed messagePublisher in _messageFeeds)
            {
                messagePublisher.StartFeed();
            }
            _log.Info("Finished Starting Host...");
        }

        public void StopHost()
        {
            _log.Info("Stopping Host...");
            foreach (IMessageFeed messagePublisher in _messageFeeds)
            {
                messagePublisher.StopFeed();
            }
            _log.Info("Finished Stopping Host");
        }


        private void InitalizeHost()
        {
            var router = _container.Resolve<IMessageRouter>();
            List<IHandleMessages> messageHandlers = _container.Resolve<IEnumerable<IHandleMessages>>().ToList();
            router.BuildHandlerRoutes(messageHandlers);
            List<IRecieveMessages> messageRecievers = _container.Resolve<IEnumerable<IRecieveMessages>>().ToList();
            InitalizeHelp(messageRecievers);
            InitalizeMessageFeeds();
            InitalizeMessageAdapters();
            router.BuildRecieverRoutes(messageRecievers);
        }

        private void InitalizeMessageAdapters()
        {
            _messageAdapters = _container.Resolve<IEnumerable<IMessageAdapter>>().ToList();
        }

        private void InitalizeMessageFeeds()
        {
            _messageFeeds = _container.Resolve<IEnumerable<IMessageFeed>>().ToList();
        }

        private void InitalizeHelp(IEnumerable<IRecieveMessages> messageRecievers)
        {
            var builer = new ContainerBuilder();

            var helpers = new Dictionary<string, HelpInformation>();

            foreach (Type recieverType in messageRecievers.Select(x => x.GetType()))
            {
                foreach (MethodInfo methodInfo in recieverType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    object[] helpAttributes = methodInfo.GetCustomAttributes(typeof(HelpAttribute), true);

                    if (!helpers.ContainsKey(recieverType.Name))
                    {
                        var helper = new HelpInformation
                                         {
                                             Plugin = recieverType.Name,
                                             Commands = new List<Command>()
                                         };

                        helpers.Add(recieverType.Name, helper);
                    }

                    foreach (HelpAttribute helpAttribute in helpAttributes)
                    {
                        helpers[recieverType.Name].Commands.Add(new Command(helpAttribute.Syntax, helpAttribute.Description,
                                                                            helpAttribute.Example));
                    }
                }
            }

            foreach (HelpInformation helpInformation in helpers.Values)
            {
                HelpInformation information = helpInformation;
                builer.Register(c => information).AsSelf().Named<HelpInformation>(information.Plugin);
            }

            builer.Update(_container.ComponentRegistry);
        }
    }
}