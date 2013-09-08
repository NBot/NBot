using System;
using System.Reflection;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HandleMessageAttribute : Attribute
    {
        public abstract IRoute CreateRoute(Type reciever, MethodInfo endpoint);
    }
}
