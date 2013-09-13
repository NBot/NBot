using System.Text.RegularExpressions;

namespace NBot.Messaging.MessageFilters
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

        #endregion
    }
}