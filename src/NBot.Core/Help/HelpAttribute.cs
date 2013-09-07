using System;

namespace NBot.Core.Help
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HelpAttribute : Attribute
    {
        public string Syntax { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }
}