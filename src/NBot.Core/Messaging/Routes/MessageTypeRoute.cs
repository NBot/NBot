using System;
using System.Reflection;

namespace NBot.Core.Messaging.Routes
{
    public class MessageTypeRoute : IRoute
    {
        private readonly Type _messageType;

        public MessageTypeRoute(Type reciever, MethodInfo endPoint, Type messageType)
        {
            _messageType = messageType;
            Reciever = reciever;
            EndPoint = endPoint;
        }

        #region IRoute Members

        public Type Reciever { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(IMessage message)
        {
            Type type = message.GetType();
            return type.IsAssignableFrom(_messageType);
        }

        public string[] GetMatchMetaData(IMessage message)
        {
            return new[] {message.GetType().Name};
        }

        #endregion
    }
}