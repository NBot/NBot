using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Sosearch : MessageHandler
    {
        [Help(Syntax = "sosearch <Search String>",
            Description = "This command will search StackOverflow.com for the provided search string and return the top five results by response count.",
            Example = "sosearch mvc")]
        [Respond("(sosearch|so me)(.*)")]
        public void SearchStackOverflow(Message message, IMessageClient client, string[] matches)
        {
            try
            {
                var authCode = Robot.GetSetting<string>("StackappsApiKey");

                if (string.IsNullOrEmpty(authCode))
                    throw new ArgumentException("Please supply the StackappsApiKey");

                var result = GetGZipedJsonServiceClient(string.Format("http://api.stackoverflow.com/1.1/search?intitle={0}&key={1}",
                                                                Uri.EscapeDataString(matches[2]),
                                                                Uri.EscapeDataString(authCode)))
                                                                .Get<SearchResult>("/");

                if (result.Questions.Any())
                {
                    client.ReplyTo(message, string.Format("Top 5 Search results for: {0}", matches[2]));

                    foreach (Question question in result.Questions.OrderByDescending(q => q.answer_count).Take(5))
                    {
                        client.ReplyTo(message, string.Format("{0} responses to: {1} {2}", question.answer_count, question.title, "http://stackoverflow.com" + question.question_answers_url));
                    }
                }
                else
                {
                    client.ReplyTo(message, string.Format("No results found for {0}", matches[2]));
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-Sosearch", ex.ToString());
                client.ReplyTo(message, "Error in searching Stack Overflow.");
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