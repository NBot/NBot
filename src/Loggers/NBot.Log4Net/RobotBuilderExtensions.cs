using NBot.Core;
using NBot.Log4Net;
namespace NBot
{
    public static class RobotBuilderExtensions
    {
        public static RobotBuilder UseLog4Net(this RobotBuilder target, string name)
        {
            return target.UseLog(new Log4NetLogger(name));
        }
    }
}
