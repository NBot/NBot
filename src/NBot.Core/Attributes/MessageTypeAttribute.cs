using System;
using System.Reflection;
using NBot.Core.Routes;

namespace NBot.Core.Attributes
{
    public class MessageTypeAttribute : HandleMessageAttribute
    {
        private readonly string _messageType;

        public MessageTypeAttribute(string messageType)
        {
            _messageType = messageType;
        }

        public override IRoute CreateRoute(IMessageHandler handler, MethodInfo endpoint)
        {
            return new MessageTypeRoute(handler, endpoint, _messageType);
        }
    }
}