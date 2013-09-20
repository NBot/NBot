using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NBot.Core.Routes
{
    public class RegexRoute : IRoute, IMessageParameterProvider
    {
        private readonly Regex _regex;
        private const string RegexParameterPattern = "{{(?<name>\\w+)}}";
        private List<string> _names = new List<string>();

        public RegexRoute(IMessageHandler handler, MethodInfo endPoint, string pattern)
        {
            string parameterizedPattern = Regex.Replace(pattern, RegexParameterPattern, match => "(?<" + match.Groups["name"].Value + ">.*)");

            _regex = new Regex(parameterizedPattern, RegexOptions.IgnoreCase);

            Handler = handler;
            EndPoint = endPoint;
        }

        #region IRoute Members

        public IMessageHandler Handler { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(Message message)
        {
            return !string.IsNullOrEmpty(message.Content) && _regex.IsMatch(message.Content);
        }



        public Dictionary<string, string> GetInputParameters(Message message)
        {
            Match match = _regex.Match(message.Content);
            return _regex.GetGroupNames().ToDictionary(name => name, name => match.Groups[name].Value.Trim());
        }

        #endregion
    }
}