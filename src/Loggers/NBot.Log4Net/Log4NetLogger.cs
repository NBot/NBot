using log4net;
using log4net.Config;
using NBot.Core.Logging;
using System;

namespace NBot.Log4Net
{
    public class Log4NetLogger : INBotLog
    {
        private readonly ILog _log;
        public Log4NetLogger(string name)
        {
            XmlConfigurator.Configure();
            _log = LogManager.GetLogger(name);
        }

        public void WriteInfo(object message, Exception exception = null)
        {
            _log.Info(message, exception);
        }

        public void WriteDebug(object message, Exception exception = null)
        {
            _log.Debug(message, exception);
        }

        public void WriteWarning(object message, Exception exception = null)
        {
            _log.Warn(message, exception);
        }

        public void WriteError(object message, Exception exception = null)
        {
            _log.Error(message, exception);
        }

        public void WriteFatal(object message, Exception exception = null)
        {
            _log.Fatal(message, exception);
        }
    }
}
