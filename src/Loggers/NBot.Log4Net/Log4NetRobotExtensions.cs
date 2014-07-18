using NBot.Log4Net;
namespace NBot.Core
{
    public static class RobotExtensions
    {
        public static IRobotConfiguration UseLog4Net(this IRobotConfiguration target, string name)
        {
            return target.UseLog(new Log4NetLogger(name));
        }
    }
}
