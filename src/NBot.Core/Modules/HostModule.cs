using Autofac;
using log4net;

namespace NBot.Core.Modules
{
    public class HostModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HostAdapter>().As<IHostAdapter>();
            builder.Register(x => new NBotHost(x, x.Resolve<ILog>())).AsSelf().SingleInstance();

            base.Load(builder);
        }
    }
}