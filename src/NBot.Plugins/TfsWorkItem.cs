using System;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;
using NBot.Plugins.TeamFoundationServer;
using log4net;

namespace NBot.Plugins
{
    public class TfsWorkItem : RecieveMessages
    {
        [Help(Syntax = "#<TicketNumber>",
            Description = "Any time a ticket number is detected, the ticket information will be returned.",
            Example = "#12345")]
        [RecieveByRegex(@"\#\d+")]
        public void TfsTicketNumberLookup(IMessage message, IHostAdapter host, string[] matches, ILog log)
        {
            try
            {
                string tfsUrl = Core.NBot.Settings["TeamFoundationUrl"] as string;

                foreach (string match in matches)
                {
                    string projectCollection = Core.NBot.Settings["TfsProjectCollection"] as string;

                    WorkItem workItem = TeamFoundationServerHelper.GetWorkItem(Convert.ToInt32(match.Substring(1)));
                    string teamProjectField = workItem.Fields.Cast<Field>().First(field => field.Name == "Team Project").Value.ToString();

                    string assignedToField = workItem.Fields.Cast<Field>().First(field => field.Name == "Assigned To").Value.ToString();
                    if (assignedToField == "")
                    {
                        assignedToField = "Unassigned";
                    }

                    string workItemTypeField = workItem.Fields.Cast<Field>().First(field => field.Name == "Work Item Type").Value.ToString();
                    string urlLink = tfsUrl + projectCollection.Substring(projectCollection.IndexOf("\\", StringComparison.Ordinal)) + "/" + teamProjectField + string.Format("/_workitems#id={0}&_a=edit", workItem.Id);

                    host.ReplyTo(message, string.Format("TFS {0} #{1} - {2} - {3}: {4} - {5}", workItemTypeField, workItem.Id, workItem.State, assignedToField, workItem.Title, urlLink));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                host.ReplyTo(message, "Ticket lookup crashed!");
            }
        }
    }
}