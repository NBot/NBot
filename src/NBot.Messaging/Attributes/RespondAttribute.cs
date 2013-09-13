using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NBot.Messaging.Routes;

namespace NBot.Messaging.Attributes
{
    public class RespondAttribute:HandleMessageAttribute
    {
        private readonly string _pattern;

        public RespondAttribute(string pattern)
        {
            string name = "nbot";
            string alias = "bot";
            _pattern = string.Format("^[@]?(?:{0}[:,]?|{1}[:,]?)\\s*(?:{2})", alias, name, pattern);
        }

        public override IRoute CreateRoute(Type handler, MethodInfo endpoint)
        {
            return new RegexRoute(handler, endpoint, _pattern);
        }
    }
}
