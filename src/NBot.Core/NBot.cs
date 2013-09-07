using System;
using System.Collections.Generic;
using Autofac;
using NBot.Core.Modules;
using Topshelf;

namespace NBot.Core
{
    public class NBot
    {
        private readonly string _name;
        private readonly string _alias;
        private readonly string _environment;
        private Host _host;
        private readonly ContainerBuilder _builder;
        public static string Name { get; private set; }
        public static string Alias { get; private set; }
        public static Dictionary<string, object> Settings { get; private set; }

        private NBot()
        {
        }

        private NBot(string name, string alias, string environment = "debug")
        {
            _name = name;
            _alias = alias;
            _environment = environment;
            _builder = new ContainerBuilder();
            Settings = new Dictionary<string, object>();
        }

        public static NBot Create(string name, string alias, string environment = "debug")
        {
            return new NBot(name, alias, environment);
        }

        public static NBot Create(string name)
        {
            return Create(name, name);
        }

        public NBot AddSetting(string key, object value)
        {
            Settings.Add(key, value);
            return this;
        }

        public NBot Register(Action<ContainerBuilder> builder)
        {
            builder(_builder);
            return this;
        }

        public void Start()
        {
            Initalize();
            _host.Run();
        }

        private void Initalize()
        {
            _builder.RegisterModule(new HostModule());
            _builder.RegisterModule(new MessagingModule());
            _builder.RegisterModule(new Log4NetModule());
            IContainer container = _builder.Build();

            _host = HostFactory.New(config =>
            {
                config.Service<NBotHost>(s =>
                {
                    s.ConstructUsing(x => container.Resolve<NBotHost>());
                    s.WhenStarted(x => x.StartHost());
                    s.WhenStopped(x => x.StopHost());
                });

                config.SetServiceName(string.Format("{0}_Service", _name));
                config.SetInstanceName(_environment);
            });
        }
    }
}
