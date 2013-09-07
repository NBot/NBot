using Autofac;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace NBot.Core.Modules
{
    public class Log4NetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            InitalizeLog4Net();
            builder.Register(b => LogManager.GetLogger(typeof(Log4NetModule))).As<ILog>();
            base.Load(builder);
        }

        private void InitalizeLog4Net()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            var consoleAppender = new ConsoleAppender();
            var fileAppender = new RollingFileAppender();

            hierarchy.Root.AddAppender(consoleAppender);
            hierarchy.Root.AddAppender(fileAppender);

            var patternLayout = new PatternLayout { ConversionPattern = "%d [%t] %-5p %c %m%n" };

            consoleAppender.Layout = patternLayout;
            fileAppender.Layout = patternLayout;
            fileAppender.File = ".\\logs\\NBot.log";
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Date | RollingFileAppender.RollingMode.Size;
            fileAppender.MaxFileSize = 10000;
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.ActivateOptions();

            hierarchy.Configured = true;
        }
    }
}
