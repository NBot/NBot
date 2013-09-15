using System.Collections.Generic;

namespace NBot.Core
{
    public interface IEntity
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}