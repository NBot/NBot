using System;
using System.Reflection;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    public class RespondByRegexAttribute : RecieveMessageAttribute
    {
        private readonly string _pattern;

        public RespondByRegexAttribute(string pattern)
        {
            string name = NBot.Name ?? "nbot";
            string alias = NBot.Alias ?? "bot";
            _pattern = string.Format("^[@]?(?:{0}[:,]?|{1}[:,]?)\\s*(?:{2})", alias, name, pattern);
            ;
        }

        public override IRoute CreateRoute(Type reciever, MethodInfo endpoint)
        {
            return new RegexRoute(reciever, endpoint, _pattern);
        }
    }
}