using System.Diagnostics;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Ping : RecieveMessages
    {
        [RespondByRegex("ping( me)? (.*)")]
        [Help(Syntax = "ping <me> <site/ip address>",Description = "Ping ip address", Example = "ping me www.google.com")]
        public void PingMe(IMessage message, IHostAdapter host, string[] matches)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "ping.exe";
            startInfo.Arguments = matches.Length == 2 ? matches[1] : matches[2];
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            var process = Process.Start(startInfo);
            process.WaitForExit(5000);
            var result = process.StandardOutput.ReadToEnd();

            host.ReplyTo(message, result);
        }
    }
}
