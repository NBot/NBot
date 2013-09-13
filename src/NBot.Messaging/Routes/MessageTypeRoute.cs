using System;
using System.Reflection;

namespace NBot.Messaging.Routes
{
    public class MessageTypeRoute : IRoute
    {
        private readonly string _messageType;

        public MessageTypeRoute(Type handler, MethodInfo endPoint, string messageType)
        {
            _messageType = messageType;
            Handler = handler;
            EndPoint = endPoint;
        }

        #region IRoute Members

        public Type Handler { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(Message message)
        {
            return message.Type == _messageType;
        }

        public string[] GetMatchMetaData(Message message)
        {
            return new[] { message.GetType().Name };
        }

        #endregion
    }
}