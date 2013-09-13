using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Core;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class FortuneMe : RecieveMessages
    {
        [RespondByRegex("(fortune)( me)?")]
        public void GetMyFortune(IMessage message, IHostAdapter host)
        {
            var body = CreateHttpClient("http://www.fortunefortoday.com/getfortuneonly.php")
                .GetAsync("")
                .Result
                .Content
                .ReadAsStringAsync()
                .Result;

            host.ReplyTo(message, body.Trim());
        }

    }
}
