using System.Collections.Generic;
using Autofac;
using NBot.Core.Messaging;
using NBot.Core.Messaging.ContentFilters;

namespace NBot.Core.Modules
{
    public class MessagingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageRouter>()
                .As<IMessageRouter>()
                .SingleInstance();

            builder.RegisterType<MessagingService>()
                .Named<IMessagingService>("messaging");

            builder.RegisterDecorator<IMessagingService>(
                (context, service) => new FilteringMessagingService(service, context.Resolve<IEnumerable<IContentFilter>>())
                , "messaging");

            base.Load(builder);
        }
    }
}