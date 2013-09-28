using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Core.Logging
{
    public class ConsoleLog : INBotLog
    {
        private static object _syncLock = new object();
        public void WriteInfo(object message, Exception exception = null)
        {
            Write("INFO", message, exception);
        }

        public void WriteDebug(object message, Exception exception = null)
        {
            Write("DEBUG", message, exception);
        }

        public void WriteWarning(object message, Exception exception = null)
        {
            Write("WARNING", message, exception);
        }

        public void WriteError(object message, Exception exception = null)
        {
            Write("ERROR", message, exception);
        }

        public void WriteFatal(object message, Exception exception = null)
        {
            Write("FATAL", message, exception);
        }

        private void Write(string level, object message, Exception exception = null)
        {
            lock (_syncLock)
            {
                Console.WriteLine("ON {0} @ {1} - {2}: {3}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), level, message);
                if (exception != null)
                {
                    WriteException(exception);
                }
            }
        }

        private void WriteException(Exception exception)
        {
            StringBuilder builder = new StringBuilder();
            Exception currentException = exception;

            do
            {
                builder.AppendLine(string.Format("EXCEPTION: {0}", currentException.Message));
                builder.AppendLine(string.Format("STACK TRACE: {0}", currentException.StackTrace));
                currentException = currentException.InnerException;
            } while (currentException != null);

            Console.WriteLine(builder);
        }
    }
}
