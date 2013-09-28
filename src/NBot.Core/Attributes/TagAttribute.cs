using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TagAttribute : Attribute
    {
        public TagAttribute(params string[] tags)
        {
            Tags = tags.Select(t => t.ToLower()).ToList();
        }

        public List<string> Tags { get; private set; }
    }
}
