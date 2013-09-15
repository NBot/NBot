using System;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;


namespace NBot.Plugins
{
    public class JoinMe : MessageHandler
    {
        [Help(Syntax = "joinme",
            Description = "This command will create a join.me meeting and post the details into the room.",
            Example = "joinme")]
        [Hear("joinme$")]
        public void CreateJoinMe(Message message, IMessageClient client)
        {
            var authCode = Robot.GetSetting<string>("JoinMeAuthCode");

            if (string.IsNullOrEmpty(authCode))
            {
                throw new ArgumentException("Please set the JoinMeAuthCode.");
            }

            var result = GetJsonServiceClient(string.Format("https://secure.join.me/API/requestCode?authCode={0}", authCode))
                .Get<String>("/");

            string[] stuff = result.Split(new[] { ':', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string code = stuff[2].Trim();
            string ticket = stuff[4].Trim();
            string presenter = string.Format("Presenter: https://secure.join.me/download.aspx?code={0}&ticket={1}", code, ticket);
            string viewer = string.Format("Viewer: http://join.me/{0}", code);


            client.ReplyTo(message, presenter);
            client.ReplyTo(message, viewer);
        }
    }
}