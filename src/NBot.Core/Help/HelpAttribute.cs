using System;
using System.Security.Cryptography;

namespace NBot.Core.Help
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class HelpAttribute : Attribute
    {
        public HelpAttribute()
        {
            Syntax = string.Empty;
            Description = string.Empty;
            Example = string.Empty;
        }

        public string Syntax { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }
}