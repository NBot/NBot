using Autofac;

namespace NBot.Plugins.Modules
{
    public class PluginsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsSelf()
                .AsImplementedInterfaces();
            base.Load(builder);
        }
    }
}