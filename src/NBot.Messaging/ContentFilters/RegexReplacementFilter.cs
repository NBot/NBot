using System.Text.RegularExpressions;

namespace NBot.Messaging.ContentFilters
{
    public class RegexReplacementFilter : IContentFilter
    {
        private readonly string _pattern;
        private readonly string _replacement;

        public RegexReplacementFilter(string pattern, string replacement)
        {
            _pattern = pattern;
            _replacement = replacement;
        }

        #region IContentFilter Members

        public bool FilterMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.Content))
                message.Content = Regex.Replace(message.Content, _pattern, _replacement);
            return false;
        }

        #endregion
    }
}