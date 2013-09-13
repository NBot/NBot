using System.Text.RegularExpressions;

namespace NBot.Messaging.MessageFilters
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

        #endregion
    }
}