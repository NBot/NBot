using System.Text.RegularExpressions;

namespace NBot.Core.MessageFilters
{
    public class RegexMessageFilter : IMessageFilter
    {
        private readonly string _pattern;

        public RegexMessageFilter(string pattern)
        {
            _pattern = pattern;
        }

        #region IMessageFilter Members

        public bool FilterMessage(Message message)
        {
            return message.Content != null && Regex.IsMatch(message.Content, _pattern);
        }

        public bool FilterMessage(ref string message)
        {
            return message != null && Regex.IsMatch(message, _pattern);
        }

        #endregion
    }
}