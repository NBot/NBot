using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    public class HandleByTypeAttribute : HandleMessageAttribute
    {
        private readonly string _messageType;

        public HandleByTypeAttribute(string messageType)
        {
            _messageType = messageType;
        }

        public override IRoute CreateRoute(Type reciever, MethodInfo endpoint)
        {
            return new MessageTypeRoute(reciever, endpoint, _messageType);
        }
    }
}
