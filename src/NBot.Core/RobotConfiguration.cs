using System.Collections.Generic;
using NBot.Core.Brains;
using NBot.Core.Logging;

namespace NBot.Core
{
    public class RobotConfiguration
    {
        public RobotConfiguration(string name, string @alias, string environment)
        {
            Name = name;
            Alias = alias;
            Log = new ConsoleLog();
            Brain = new SimpleBrain();
            Environment = environment;
            Settings = new Dictionary<string, object>();
        }

        public string Name { get; set; }
        public string Alias { get; set; }
        public string Environment { get; set; }
        public IBrain Brain { get; set; }
        public INBotLog Log { get; set; }
        public Dictionary<string, object> Settings { get; set; }


    }
}