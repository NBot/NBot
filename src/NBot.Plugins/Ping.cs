using System.Diagnostics;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;


namespace NBot.Plugins
{
    public class Ping : MessageHandler
    {
        [Respond("ping( me)? (.*)")]
        [Help(Syntax = "ping <me> <site/ip address>",Description = "Ping ip address", Example = "ping me www.google.com")]
        public void PingMe(Message message, IMessageClient client, string[] matches)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "ping.exe";
            startInfo.Arguments = matches.Length == 2 ? matches[1] : matches[2];
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            var process = Process.Start(startInfo);
            process.WaitForExit(5000);
            var result = process.StandardOutput.ReadToEnd();

            client.ReplyTo(message, result);
        }
    }
}
