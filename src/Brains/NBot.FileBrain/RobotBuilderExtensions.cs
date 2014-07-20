﻿using NBot.Core;
using NBot.Core.Brains;

namespace NBot
{
    public static class RobotBuilderExtensions
    {
        public static RobotBuilder UseFileBrain(this RobotBuilder configuration)
        {
            return configuration.UseFileBrain(".\\Brain");
        }

        public static RobotBuilder UseFileBrain(this RobotBuilder configuration, string filePath)
        {
            return configuration.UseBrain(new FileBrain(filePath));
        }
    }
}
