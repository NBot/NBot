using System.Text.RegularExpressions;
using NBot.Core.Brains;

namespace NBot.Core.Messaging.ContentFilters
{
    public class BrainDataContentFilter : IContentFilter
    {
        private readonly IBrain _brain;

        public BrainDataContentFilter(IBrain brain)
        {
            _brain = brain;
        }

        private const string ParameterReplacementPattern = "{{(.*)}}";

        public bool FilterMessage(IMessage message)
        {


            message.Content = message.Content == null ? null : Regex.Replace(message.Content, ParameterReplacementPattern, delegate(Match match)
                                                                             {
                                                                                 var key = match.Groups[1].Value;
                                                                                 return _brain.ContainsKey(key) ? _brain.GetValue(key).ToString() : "{{Key not found}}";
                                                                             });
            return false;
        }
    }
}
