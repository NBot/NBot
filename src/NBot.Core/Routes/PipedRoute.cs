using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NBot.Core.Routes
{
    public class PipedRoute : IRoute, IPipedParameterProvider
    {
        private readonly string _commandName;
        private readonly string[] _parameterNames;

        public PipedRoute(IMessageHandler handler, MethodInfo endpoint, string commandName, string[] parameterNames)
        {
            _commandName = commandName;
            _parameterNames = parameterNames;
            Handler = handler;
            EndPoint = endpoint;
        }

        public IMessageHandler Handler { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(Message message)
        {
            return message.Content.Equals(_commandName);
        }

        public Dictionary<string, string> GetInputParameters(string[] values)
        {
            var result = new Dictionary<string, string>();

            if (values.Length != _parameterNames.Length)
                throw new ArgumentException("There are not the same number of input values as there are input parameters.");

            for (int parameterIndex = 0; parameterIndex < _parameterNames.Length; parameterIndex++)
            {
                result.Add(_parameterNames[parameterIndex], values[parameterIndex]);
            }

            return result;
        }
    }
}
