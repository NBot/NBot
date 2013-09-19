using System;
using System.Collections.Generic;
using System.Reflection;

namespace NBot.Core.Routes
{
    public class MessageTypeRoute : IRoute, IMessageParameterProvider
    {
        private readonly string _messageType;

        public MessageTypeRoute(IMessageHandler handler, MethodInfo endPoint, string messageType)
        {
            _messageType = messageType;
            Handler = handler;
            EndPoint = endPoint;
        }

        #region IRoute Members

        public IMessageHandler Handler { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(Message message)
        {
            return message.Type == _messageType;
        }

        public Dictionary<string, string> GetInputParameters(Message message)
        {
            return new Dictionary<string, string>();
        }

        #endregion
    }
}