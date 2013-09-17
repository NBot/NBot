using System;
using System.Collections.Generic;
using System.Reflection;

namespace NBot.Core.Routes
{
    public interface IRoute
    {
        IMessageHandler Handler { get; }
        MethodInfo EndPoint { get; }
        bool IsMatch(Message message);
        Dictionary<string, string> GetInputParameters(Message message);
    }
}