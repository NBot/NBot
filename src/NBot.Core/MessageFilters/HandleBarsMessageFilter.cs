using System.Text.RegularExpressions;
using NBot.Core.Brains;

namespace NBot.Core.MessageFilters
{
    public class HandleBarsMessageFilter : IMessageFilter
    {
        private readonly IBrain _brain;

        public HandleBarsMessageFilter(IBrain brain)
        {
            _brain = brain;
        }

        private const string ParameterReplacementPattern = "{{(.*)}}";

        public bool FilterMessage(Message message)
        {
            string content = message.Content;
            var result = FilterMessage(ref content);
            message.Content = content;
            return result;
        }

        public bool FilterMessage(ref string message)
        {
            message = message == null ? null : Regex.Replace(message, ParameterReplacementPattern, delegate(Match match)
            {
                var key = match.Groups[1].Value;
                return _brain.ContainsKey(key) ? _brain.GetValue(key).ToString() : "{{Key not found}}";
            });

            return false;
        }


    }
}
