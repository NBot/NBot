using System;
using System.Reflection;

namespace NBot.Core.Routes
{
    public interface IRoute
    {
        IMessageHandler Handler { get; }
        MethodInfo EndPoint { get; }
        bool IsMatch(Message message);
    }
}