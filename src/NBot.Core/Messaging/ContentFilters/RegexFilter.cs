using System.Text.RegularExpressions;

namespace NBot.Core.Messaging.ContentFilters
{
    public class RegexFilter : IContentFilter
    {
        private readonly string _pattern;

        public RegexFilter(string pattern)
        {
            _pattern = pattern;
        }

        #region IContentFilter Members

        public bool FilterMessage(IMessage message)
        {
            return message.Content != null && Regex.IsMatch(message.Content, _pattern);
        }

        #endregion
    }
}