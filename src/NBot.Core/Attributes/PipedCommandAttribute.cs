using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NBot.Core.Routes;

namespace NBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PipedCommandAttribute : HandleMessageAttribute
    {
        private readonly string _commandName;
        private readonly string[] _parameterNames;

        public PipedCommandAttribute(string commandName, params string[] parameterNames)
        {
            _commandName = commandName;
            _parameterNames = parameterNames;
        }

        public override IRoute CreateRoute(IMessageHandler handler, MethodInfo endpoint)
        {
            return new PipedRoute(handler, endpoint, _commandName, _parameterNames);
        }
    }
}
