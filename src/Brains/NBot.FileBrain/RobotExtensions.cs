using NBot.Core;
using NBot.Core.Brains;

namespace NBot
{
    public static class RobotExtensions
    {
        public static IRobotConfiguration UseFileBrain(this IRobotConfiguration configuration)
        {
            return configuration.UseFileBrain(".\\Brain");
        }

        public static IRobotConfiguration UseFileBrain(this IRobotConfiguration configuration, string filePath)
        {
            return configuration.UseBrain(new FileBrain(filePath));
        }
    }
}
