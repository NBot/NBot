using System.Text.RegularExpressions;

namespace NBot.Core.MessageFilters
{
    public class RegexReplacementMessageFilter : IMessageFilter
    {
        private readonly string _pattern;
        private readonly string _replacement;

        public RegexReplacementMessageFilter(string pattern, string replacement)
        {
            _pattern = pattern;
            _replacement = replacement;
        }

        #region IMessageFilter Members

        public bool FilterMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Content))
                message.Content = Regex.Replace(message.Content, _pattern, _replacement);
            return false;
        }

        public bool FilterMessage(ref string message)
        {
            if (!string.IsNullOrEmpty(message))
                message = Regex.Replace(message, _pattern, _replacement);
            return false;
        }

        #endregion
    }
}