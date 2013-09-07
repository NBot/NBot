using System.Collections.Generic;

namespace NBot.Core
{
    public interface INBotSettings
    {
        string Name { get; }
        string Alias { get; }
        Dictionary<string, object> Settings { get; }
    }
}