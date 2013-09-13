using System;
using System.Reflection;

namespace NBot.Messaging.Routes
{
    public interface IRoute
    {
        Type Handler { get; }
        MethodInfo EndPoint { get; }
        bool IsMatch(Message message);
        string[] GetMatchMetaData(Message message);
    }
}
