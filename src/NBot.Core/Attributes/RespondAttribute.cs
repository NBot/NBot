using System;
using System.Reflection;
using NBot.Core.Routes;

namespace NBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RespondAttribute : HandleMessageAttribute
    {
        private readonly string _pattern;

        public RespondAttribute(string pattern)
        {
            _pattern = string.Format("^[@]?(?:{0}[:,]?|{1}[:,]?)\\s*(?:{2})", Robot.Alias, Robot.Name, pattern);
        }

        public override IRoute CreateRoute(IMessageHandler handler, MethodInfo endpoint)
        {
            return new RegexRoute(handler, endpoint, _pattern);
        }
    }
}