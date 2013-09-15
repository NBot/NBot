using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBot.Core.Attributes;
using ServiceStack.Messaging;

namespace NBot.Core.Help
{
    public class Help : MessageHandler
    {
        private readonly IEnumerable<HelpInformation> _helpInformation;

        public Help(IEnumerable<HelpInformation> helpInformation)
        {
            _helpInformation = helpInformation;
        }

        [Respond("(help|commands)(.*)")]
        public void HelpCommand(Message message, IMessageClient client, string[] matches)
        {
            try
            {
                if (matches.Count() <= 2)
                {
                    MainHelpMenu(message, client);
                }
                else
                {
                    PluginHelp(message, client, matches[2]);
                }
            }
            catch (Exception ex)
            {
                client.ReplyTo(message, "The Help plugin blew up. We're all in trouble now!");
            }
        }

        private void MainHelpMenu(Message message, IMessageClient client)
        {
            string response = _helpInformation.Aggregate("List of Plugins. Please use help <plugin> for more information.", (current, helpInformation) => current + ("\n\t-" + helpInformation.Plugin));
            client.ReplyTo(message, response);
        }

        private void PluginHelp(Message message, IMessageClient client, string plugIn)
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

            client.ReplyTo(message, helpInformationCommands.Any()
                                      ? ouputMessage
                                      : string.Format("Plugin '{0}' not found.", plugIn));
        }
    }
}