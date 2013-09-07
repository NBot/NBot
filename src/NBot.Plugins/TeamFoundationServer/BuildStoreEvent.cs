using Microsoft.TeamFoundation.Build.Client;

namespace NBot.Plugins.TeamFoundationServer
{
    public enum BuildStoreEventType
    {
        Build,
        QualityChanged
    }

    public class BuildStoreEvent
    {
        public BuildStoreEventType Type { get; set; }
        public IBuildDetail Data { get; set; }
    }
}