using System.Collections.Generic;
using NBot.Core;

namespace NBot.ConsoleAdapter
{
    public class ConsoleEntity : IEntity
    {
        public ConsoleEntity(string id, string name )
        {
            Id = id;
            Name = name;
            Metadata = new Dictionary<string, object>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Metadata { get; private set; }
    }
}