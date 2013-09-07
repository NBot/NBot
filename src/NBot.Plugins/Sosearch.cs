using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using NBot.Core;
using NBot.Core.Extensions;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Sosearch : RecieveMessages
    {
        [Help(Syntax = "sosearch <Search String>",
            Description = "This command will search StackOverflow.com for the provided search string and return the top five results by response count.",
            Example = "sosearch mvc")]
        [RespondByRegex("(sosearch|so me)(.*)")]
        public void SearchStackOverflow(IMessage message, IHostAdapter host, string[] matches)
        {
            try
            {
                string authCode = Core.NBot.Settings["AckappsApiKey"] as string;

                HttpClient client = CreateCompressionHttpClient(
                    string.Format("http://api.stackoverflow.com/1.1/search?intitle={0}&key={1}",
                                  Uri.EscapeDataString(matches[2]),
                                  Uri.EscapeDataString(authCode)));

                var result = client.GetAsync("").Result.Content.ReadAsStringAsync().Result.FromJson<SearchResult>();

                if (result.Questions.Any())
                {
                    host.ReplyTo(message, string.Format("Top 5 Search results for: {0}", matches[2]));

                    foreach (Question question in result.Questions.OrderByDescending(q => q.answer_count).Take(5))
                    {
                        host.ReplyTo(message, string.Format("{0} responses to: {1} {2}", question.answer_count, question.title, "http://stackoverflow.com" + question.question_answers_url));
                    }
                }
                else
                {
                    host.ReplyTo(message, string.Format("No results found for {0}", matches[2]));
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-Sosearch", ex.ToString());
                host.ReplyTo(message, "Error in searching Stack Overflow.");
            }
        }

        #region Nested type: Question

        class Question
        {
            public string title { get; set; }
            public int answer_count { get; set; }
            public string question_answers_url { get; set; }
        }

        #endregion

        #region Nested type: SearchResult

        class SearchResult
        {
            public List<Question> Questions { get; set; }
        }

        #endregion
    }
}