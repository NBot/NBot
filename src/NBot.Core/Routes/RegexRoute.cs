using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NBot.Core.Routes
{
    public class RegexRoute : IRoute
    {
        private readonly string _pattern;
        private const string RegexParameterPattern = "{{(?<name>\\w+)}}";
        private List<string> _names = new List<string>();

        public RegexRoute(IMessageHandler handler, MethodInfo endPoint, string pattern)
        {
            _pattern = Regex.Replace(pattern, RegexParameterPattern, match =>
                                                                         {
                                                                             _names.Add(match.Groups["name"].Value);
                                                                             return "(?<" + match.Groups["name"].Value + ">.*)";
                                                                         });
            Handler = handler;
            EndPoint = endPoint;
        }

        #region IRoute Members

        public IMessageHandler Handler { get; private set; }
        public MethodInfo EndPoint { get; private set; }

        public bool IsMatch(Message message)
        {
            return !string.IsNullOrEmpty(message.Content) &&
                   Regex.IsMatch(message.Content, _pattern, RegexOptions.IgnoreCase);
        }



        public Dictionary<string, string> GetInputParameters(Message message)
        {
            Match match = Regex.Match(message.Content, _pattern, RegexOptions.IgnoreCase);
            return _names.ToDictionary(name => name, name => match.Groups[name].Value);
        }

        #endregion
    }
}