using System;
using System.Reflection;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    public class RecieveByRegexAttribute : RecieveMessageAttribute
    {
        private readonly string _regex;

        public RecieveByRegexAttribute(string regex)
        {
            _regex = regex;
        }

        public override IRoute CreateRoute(Type reciever, MethodInfo endpoint)
        {
            return new RegexRoute(reciever, endpoint, _regex);
        }
    }
}