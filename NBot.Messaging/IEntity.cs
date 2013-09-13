using System.Collections.Generic;

namespace NBot.Messaging
{
    public interface IEntity
    {
        string Id { get; set; }
        string Name { get; set; }
        Dictionary<string,object> Metadata { get; } 
    }
}