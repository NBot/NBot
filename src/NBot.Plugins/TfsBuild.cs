using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;
using NBot.Plugins.TeamFoundationServer;

namespace NBot.Plugins
{
    public class TfsBuild : RecieveMessages
    {
        private readonly List<int> _roomsIds = new List<int>();

        public TfsBuild(IHostAdapter host)
        {
            string roomsConfiguration = Core.NBot.Settings["AnnounceRooms"] as string;

            if (!string.IsNullOrEmpty(roomsConfiguration))
            {
                _roomsIds = roomsConfiguration.Contains("*")
                                ? host.GetAllRooms().Select(room => room.Id).ToList()
                                : roomsConfiguration.Split(new[] { ';', ',' }).Select(int.Parse).ToList();
            }
        }


        [Help(Syntax = "build queue <BuildDefinition>",
            Description = "This command is used to queue a new build for the specified build definition.",
            Example = "build queue River.Zombie")]
        [RespondByRegex("(build queue)(.*)")]
        public void BuildQueue(IMessage message, IHostAdapter host, string[] matches)
        {
            string buildResponse = TeamFoundationServerHelper.QueueBuild(matches[2]);
            host.ReplyTo(message, buildResponse);
        }


        [Help(Syntax = "build status <BuildDefinition>",
            Description = "This command will return the current status of the specified build definition. The information that will be returned status," +
                          "the time it was completed or started, and the person who triggered the build.",
            Example = "build status River.Zombie")]
        [RespondByRegex("(build status)(.*)")]
        public void BuildStatus(IMessage message, IHostAdapter host, string[] matches)
        {
            try
            {
                if (matches.Count() == 3)
                {
                    string buildDefinition = matches[2];
                    var buildDefinitions = new List<string> { buildDefinition };
                    List<BuildStoreEvent> buildStoreEvents = TeamFoundationServerHelper.GetListOfBuildStoreEvents(buildDefinitions).ToList();

                    if (buildStoreEvents.Any())
                    {
                        IBuildDetail firstBuildStoreEvent = buildStoreEvents.First().Data;
                        string timeStamp = firstBuildStoreEvent.FinishTime.ToString();

                        if (timeStamp == "1/1/0001 12:00:00 AM")
                        {
                            timeStamp = firstBuildStoreEvent.StartTime.ToString();
                        }

                        string buildMessage = string.Format("Build Status for {0}: {1} at {2} requested by {3}", buildDefinition, firstBuildStoreEvent.Status.ToString(), timeStamp, firstBuildStoreEvent.RequestedBy);
                        Announce(host, buildMessage);
                    }
                    else
                    {
                        Announce(host, string.Format("No build found for: {0}", buildDefinition));
                    }
                }
                else
                {
                    Announce(host, "Please specify build definition!");
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("NBot-BuildStatus", ex.ToString());
                host.ReplyTo(message, "Build status crashed!");
            }
        }

        private void Announce(IHostAdapter host, string message)
        {
            foreach (int roomId in _roomsIds)
            {
                host.Send(roomId, message);
            }
        }
    }
}