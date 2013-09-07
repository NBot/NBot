using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace NBot.Plugins.TeamFoundationServer
{
    public class TeamFoundationServerHelper
    {
        #region Main Methods

        public static T GetService<T>()
        {
            string teamFoundationServerUrl = Core.NBot.Settings["TeamFoundationUrl"] as string;

            T service;
            try
            {
                string username = Core.NBot.Settings["TfsUserName"] as string;
                string password = Core.NBot.Settings["TfsPassword"] as string;
                string domain = Core.NBot.Settings["TfsDomain"] as string;
                string projectCollection = Core.NBot.Settings["TfsProjectCollection"] as string;

                ConnectByImplementingCredentialsProvider connect = new ConnectByImplementingCredentialsProvider(username, domain, password);

                TfsConfigurationServer configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(new Uri(teamFoundationServerUrl), connect);

                configurationServer.EnsureAuthenticated();

                CatalogNode catalogNode = configurationServer.CatalogNode;

                ReadOnlyCollection<CatalogNode> tpcNodes = catalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

                Guid tpcIdMatch = new Guid();
                foreach (Guid tpcId in tpcNodes.Select(tpcNode => new Guid(tpcNode.Resource.Properties["InstanceId"])).Where(tpcId => configurationServer.GetTeamProjectCollection(tpcId).Name == projectCollection))
                {
                    tpcIdMatch = tpcId;
                }

                var teamProjectCollection = configurationServer.GetTeamProjectCollection(tpcIdMatch);
                service = (T)teamProjectCollection.GetService(typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to connect to TFS - {0}", ex));
            }

            return service;
        }

        public static IEnumerable<string> GetListOfTeamProjects()
        {
            var structureService = GetService<ICommonStructureService>();

            if (structureService == null)
            {
                return new List<string>().ToArray();
            }

            ProjectInfo[] projectInfoArray = structureService.ListProjects();

            string[] projects = Array.ConvertAll(
                projectInfoArray,
                info => info.Name
                );

            return projects;
        }

        #endregion

        #region Build Methods

        private static readonly Dictionary<string, IBuildDetail> _cacheLookup = new Dictionary<string, IBuildDetail>();

        public static List<BuildStoreEvent> GetListOfBuildStoreEvents(List<string> buildDefinitionNames)
        {
            IEnumerable<string> teamProjects = GetListOfTeamProjects();
            IEnumerable<IBuildDetail> builds = GetBuildsForTeamProject(teamProjects, buildDefinitionNames);

            return builds.Select(GetBuildStoreEventIfAny).Where(buildStoreEvent => buildStoreEvent != null).ToList();
        }

        private static BuildStoreEvent GetBuildStoreEventIfAny(IBuildDetail build)
        {
            BuildStoreEvent buildStoreEvent;
            if (!_cacheLookup.ContainsKey(build.Uri.AbsoluteUri))
            {
                _cacheLookup.Add(build.Uri.AbsoluteUri, build);

                buildStoreEvent = new BuildStoreEvent
                                      {
                                          Type = BuildStoreEventType.Build,
                                          Data = build
                                      };
                return buildStoreEvent;
            }

            IBuildDetail originalBuild = _cacheLookup[build.Uri.AbsoluteUri];
            _cacheLookup[build.Uri.AbsoluteUri] = build;

            if (originalBuild.Quality != build.Quality || originalBuild.Status != build.Status)
            {
                buildStoreEvent = new BuildStoreEvent
                                      {
                                          Data = build,
                                          Type = originalBuild.Quality != build.Quality
                                                     ? BuildStoreEventType.QualityChanged
                                                     : BuildStoreEventType.Build
                                      };
                return buildStoreEvent;
            }

            return null;
        }

        private static IEnumerable<IBuildDetail> GetBuildsForTeamProject(IEnumerable<string> teamProjects,
                                                                         List<string> buildDefinitionNames)
        {
            var buildList = new List<IBuildDetail>();
            var buildServer = GetService<IBuildServer>();
            foreach (
                var tempDefs in
                    teamProjects.Select(buildServer.QueryBuildDefinitions).Select(
                        definitions =>
                        definitions.Where(
                            buildDefinition =>
                            buildDefinitionNames.Contains(buildDefinition.Name.ToString(CultureInfo.InvariantCulture))).
                            ToList()))
            {
                buildList.AddRange((GetBuildDetail(buildServer, tempDefs)));
            }
            return buildList;
        }

        private static IEnumerable<IBuildDetail> GetBuildDetail(IBuildServer buildServer,
                                                                IEnumerable<IBuildDefinition> tempDefs)
        {
            var buildList = new List<IBuildDetail>();
            foreach (IBuildDefinition definition in tempDefs)
            {
                if (definition.QueueStatus == DefinitionQueueStatus.Enabled)
                {
                    IBuildDetail[] builds = buildServer.QueryBuilds(definition);
                    IBuildDetail buildToAdd = null;
                    foreach (
                        IBuildDetail build in builds.Where(build => buildToAdd == null || build.StartTime > buildToAdd.StartTime)
                        )
                    {
                        buildToAdd = build;
                    }
                    if (buildToAdd != null)
                    {
                        buildList.Add(buildToAdd);
                    }
                }
            }

            return buildList;
        }

        public static string QueueBuild(string buildDefinitionName)
        {
            try
            {
                var buildService = GetService<IBuildServer>();
                IEnumerable<string> teamProjects = GetListOfTeamProjects();
                IBuildDefinition buildDef = buildService.GetBuildDefinition(teamProjects.First(), buildDefinitionName);
                buildService.QueueBuild(buildDef);
                return string.Format("Build for {0} has been queued.", buildDefinitionName);
            }
            catch (Exception)
            {
                return string.Format("There was an error in queuing a build for {0}", buildDefinitionName);
            }
        }

        #endregion

        #region Work Item Methods

        public static WorkItem GetWorkItem(int workItemId)
        {
            var workItemStore = GetService<WorkItemStore>();
            return workItemStore.GetWorkItem(workItemId);
        }

        #endregion
    }
}