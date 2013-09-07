using System;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class JoinMe : RecieveMessages
    {
        [Help(Syntax = "joinme",
            Description = "This command will create a join.me meeting and post the details into the room.",
            Example = "joinme")]
        [RecieveByRegex("joinme$")]
        public void CreateJoinMe(IMessage message, IHostAdapter host)
        {
            string authCode = Core.NBot.Settings["JoinMeAuthCode"] as string;

            string result = CreateHttpClient(string.Format("https://secure.join.me/API/requestCode?authCode={0}", authCode))
                .GetAsync("")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result;

            string[] stuff = result.Split(new[] { ':', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string code = stuff[2].Trim();
            string ticket = stuff[4].Trim();
            string presenter = string.Format("Presenter: https://secure.join.me/download.aspx?code={0}&ticket={1}", code, ticket);
            string viewer = string.Format("Viewer: http://join.me/{0}", code);


            host.ReplyTo(message, presenter);
            host.ReplyTo(message, viewer);
        }
    }
}