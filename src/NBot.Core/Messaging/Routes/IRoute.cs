using System;
using System.Reflection;

namespace NBot.Core.Messaging.Routes
{
    public interface IRoute
    {
        Type Reciever { get; }
        MethodInfo EndPoint { get; }
        bool IsMatch(IMessage message);
        string[] GetMatchMetaData(IMessage message);
    }
}