using System.Diagnostics;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Ping : MessageHandler
    {
        [Respond("ping( me)? {{location}}")]
        [Help(Syntax = "ping <me> <site/ip address>", Description = "Ping ip address", Example = "ping me www.google.com")]
        [PipedCommand("ping", "location")]
        public void PingMe(Message message, IMessageClient client, string location)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "ping.exe";
            startInfo.Arguments = location;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            var process = Process.Start(startInfo);
            process.WaitForExit(5000);
            var result = process.StandardOutput.ReadToEnd();

            client.ReplyTo(message, result);
        }
    }
}
