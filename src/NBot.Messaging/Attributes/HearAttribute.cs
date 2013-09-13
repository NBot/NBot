using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NBot.Messaging.Routes;

namespace NBot.Messaging.Attributes
{
    public class HearAttribute:HandleMessageAttribute
    {
        private readonly string _pattern;

        public HearAttribute(string pattern)
        {
            _pattern = pattern;
        }

        public override IRoute CreateRoute(Type handler, MethodInfo endpoint)
        {
            return new RegexRoute(handler,endpoint,_pattern);
        }
    }
}
