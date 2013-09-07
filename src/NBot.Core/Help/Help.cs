using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Core.Help
{
    public class Help : RecieveMessages
    {
        private readonly IEnumerable<HelpInformation> _helpInformation;

        public Help(IEnumerable<HelpInformation> helpInformation)
        {
            _helpInformation = helpInformation;
        }

        [RespondByRegex("(help|commands)(.*)")]
        public void HelpCommand(IMessage message, IHostAdapter host, string[] matches)
        {
            try
            {
                if (matches.Count() <= 2)
                {
                    MainHelpMenu(message, host);
                }
                else
                {
                    PluginHelp(message, host, matches[2]);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-Help", ex.ToString());
                host.ReplyTo(message, "The Help plugin blew up. We're all in trouble now!");
            }
        }

        private void MainHelpMenu(IMessage message, IHostAdapter host)
        {
            string response = _helpInformation.Aggregate("List of Plugins. Please use help <plugin> for more information.", (current, helpInformation) => current + ("\n\t-" + helpInformation.Plugin));
            host.ReplyTo(message, response);
        }

        private void PluginHelp(IMessage message, IHostAdapter host, string plugIn)
        {
            string ouputMessage = string.Format("Commands for {0}:", plugIn);
            IEnumerable<List<Command>> helpInformationCommands = _helpInformation.Where(hi => hi.Plugin.ToLower() == plugIn.ToLower()).Select(hi => hi.Commands);

            foreach (var commands in _helpInformation.Where(hi => hi.Plugin.ToLower() == plugIn.ToLower()).Select(hi => hi.Commands))
            {
                foreach (Command command in commands)
                {
                    ouputMessage += "\n" + string.Format("Command Syntax: {0}\nDescription: {1}\nExample: {2}\n", command.Syntax, command.Description, command.Example);
                }
            }

            host.ReplyTo(message, helpInformationCommands.Any()
                                      ? ouputMessage
                                      : string.Format("Plugin '{0}' not found.", plugIn));
        }
    }
}