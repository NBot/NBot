using System;

namespace NBot.Core.Logging
{
    public interface INBotLog
    {
        void WriteInfo(object message, Exception exception = null);
        void WriteDebug(object message, Exception exception = null);
        void WriteWarning(object message, Exception exception = null);
        void WriteError(object message, Exception exception = null);
        void WriteFatal(object message, Exception exception = null);
    }
}
