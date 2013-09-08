using System;
using System.Net.Http;
using System.Text;
using Autofac;
using NBot.Campfire.Messages;
using NBot.Core;
using NBot.Core.Messaging;

namespace NBot.Campfire.Modules
{
    public class CampfireModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string token = Core.NBot.Settings["Token"] as string;
            string account = Core.NBot.Settings["Account"] as string;
            string auth = string.Format("{0}:X", token);
            string authHeader = string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(auth)));
            var client = new HttpClient { BaseAddress = new Uri(string.Format("https://{0}.campfirenow.com/", account)) };

            client.DefaultRequestHeaders.Add("Authorization", authHeader);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .As<IRecieveMessages>()
                .AsSelf();

            builder.RegisterType<CampfireMessageAdapter>()
                .As<IMessageAdapter>();

            builder.RegisterType<CampfireFeed>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .As<IMessage>();

            builder.Register(x => client)
                .AsSelf()
                .Named<HttpClient>("CampfireClient");

            builder.Register(c => c.Resolve<IMessagingService>().Send<Account>(CampfireMessageFactory.CreateGetAccountMessage())).SingleInstance();

            builder.Register(c => c.Resolve<IMessagingService>().Send<User>(CampfireMessageFactory.CreateGetMyUserMessage())).SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .As<IHandleMessages>()
                .AsSelf();

            base.Load(builder);
        }
    }
}