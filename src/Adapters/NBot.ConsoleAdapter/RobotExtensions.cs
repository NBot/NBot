using NBot.Core;

namespace NBot
{
    public static class RobotExtensions
    {
        public static IRobotConfiguration UseConsoleAdapter(this IRobotConfiguration configuration)
        {
            return configuration.RegisterAdapter(new Adapters.ConsoleAdapter(), "ConsoleChannel");
        }
    }
}
