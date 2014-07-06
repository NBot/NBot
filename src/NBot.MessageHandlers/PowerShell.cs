using NBot.Core;
using NBot.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NBot.MessageHandlers
{
    [Tag("Productivity")]
    public class PowerShell : MessageHandler
    {
        private readonly Dictionary<string, string> _commands;

        public PowerShell(Action<Dictionary<string, string>> build)
        {
            _commands = new Dictionary<string, string>();
            build(_commands);
        }
       
        [Respond("exec {{command}}\\((?<parameters>(\"?[\\w\\d]+\"?)(,\\s*\"?[\\w\\d]+\"?)*)\\)")]
        [Respond("exec {{command}}")]
        public void HandlePowerShellCommand(Message message, IMessageClient client, string command, string parameters)
        {
            if (_commands.ContainsKey(command))
            {
                var filePath = _commands[command];
                var scriptParameters = string.IsNullOrEmpty(parameters) ? string.Empty : parameters;

                var startInfo = new ProcessStartInfo();
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = string.Format("-ExecutionPolicy RemoteSigned -File {0} {1}", filePath, scriptParameters);
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                var process = Process.Start(startInfo);
                process.WaitForExit(5000);
                var result = process.StandardOutput.ReadToEnd();
                client.ReplyTo(message, result);
            }
        }
    }
}
