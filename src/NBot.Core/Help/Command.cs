namespace NBot.Core.Help
{
    public class Command
    {
        public Command(string syntax, string description, string example)
        {
            Syntax = syntax;
            Description = description;
            Example = example;
        }

        public string Syntax { get; set; }
        public string Description { get; set; }
        public string Example { get; set; }
    }
}