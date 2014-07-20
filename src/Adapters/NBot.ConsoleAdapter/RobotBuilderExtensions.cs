using NBot.Core;

namespace NBot
{
    public static class RobotBuilderExtensions
    {
        public static RobotBuilder UseConsoleAdapter(this RobotBuilder builder)
        {
            return builder.RegisterAdapter( "ConsoleChannel", new Adapters.ConsoleAdapter());
        }
    }
}
