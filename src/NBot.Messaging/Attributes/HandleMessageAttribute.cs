using System;
using System.Reflection;
using NBot.Messaging.Routes;

namespace NBot.Messaging.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HandleMessageAttribute: Attribute
    {
        public abstract IRoute CreateRoute(Type handler, MethodInfo endpoint);
    }
}
