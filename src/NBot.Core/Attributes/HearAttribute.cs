using System;
using System.Reflection;
using NBot.Core.Routes;

namespace NBot.Core.Attributes
{
    public class HearAttribute : HandleMessageAttribute
    {
        private readonly string _pattern;

        public HearAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override IRoute CreateRoute(IMessageHandler handler, MethodInfo endpoint)
        {
            return new RegexRoute(handler, endpoint, _pattern);
        }
    }
}