using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NBot.Messaging.Routes;

namespace NBot.Messaging.Attributes
{
    public class MessageTypeAttribute : HandleMessageAttribute
    {
        private readonly string _messageType;

        public MessageTypeAttribute(string messageType)
        {
            _messageType = messageType;
        }

        public override IRoute CreateRoute(Type handler, MethodInfo endpoint)
        {
            return new MessageTypeRoute(handler, endpoint, _messageType);
        }
    }
}
