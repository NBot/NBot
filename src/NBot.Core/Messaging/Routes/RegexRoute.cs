using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NBot.Core.Messaging.Routes
{
    public class RegexRoute : IRoute
    {
        private readonly string _regex;

        public RegexRoute(Type reciever, MethodInfo endPoint, string regex)
        {
            _regex = regex;
            Reciever = reciever;
            EndPoint = endPoint;
        }

        #region IRoute Members

        public Type Reciever { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(IMessage message)
        {
            return !string.IsNullOrEmpty(message.Content) && Regex.IsMatch(message.Content, _regex, RegexOptions.IgnoreCase);
        }

        public string[] GetMatchMetaData(IMessage message)
        {
            Match match = Regex.Match(message.Content, _regex, RegexOptions.IgnoreCase);

            var result = new List<string>();

            for (int groupIndex = 0; groupIndex < match.Groups.Count; groupIndex++)
            {
                string value = match.Groups[groupIndex].Value.Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    result.Add(value);
                }
            }
            return result.ToArray();
        }

        #endregion
    }
}