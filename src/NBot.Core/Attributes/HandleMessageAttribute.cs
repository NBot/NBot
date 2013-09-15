using System;
using System.Reflection;
using NBot.Core.Routes;

namespace NBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HandleMessageAttribute : Attribute
    {
        public abstract IRoute CreateRoute(IMessageHandler handler, MethodInfo endpoint);
    }
}