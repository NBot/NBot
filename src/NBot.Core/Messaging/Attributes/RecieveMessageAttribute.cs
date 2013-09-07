using System;
using System.Reflection;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class RecieveMessageAttribute : Attribute
    {
        public abstract IRoute CreateRoute(Type reciever, MethodInfo endpoint);
    }
}