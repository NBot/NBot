using System;
using System.Reflection;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    public class RecieveByTypeAttribute : RecieveMessageAttribute
    {
        private readonly Type _type;

        public RecieveByTypeAttribute(Type type)
        {
            _type = type;
        }

        public override IRoute CreateRoute(Type reciever, MethodInfo endpoint)
        {
            return new MessageTypeRoute(reciever, endpoint, _type);
        }
    }
}