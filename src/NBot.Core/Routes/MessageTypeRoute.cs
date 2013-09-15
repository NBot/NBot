using System;
using System.Reflection;

namespace NBot.Core.Routes
{
    public class MessageTypeRoute : IRoute
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

        public string[] GetMatchMetaData(Message message)
        {
            return new[] {message.GetType().Name};
        }

        #endregion
    }
}