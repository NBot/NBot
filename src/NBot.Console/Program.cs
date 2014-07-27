using System.Reflection;
using NBot.Core;
using NBot.Core.MessageFilters;

namespace NBot.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Robot.Create()
                .UseFileBrain() // <- Pick a brain or the robot will choose one for you
                .UseConsoleAdapter() // <- Pick an adapter or the robot will choose one for you
                .RegisterMessageFilter(rc => new HandleBarsMessageFilter(rc.Brain)) // <- Register zero or more Message Filters
                .RegisterHandlersInAssembly(Assembly.Load("NBot.MessageHandlers")) // <- Register all the Handlers
                .AllowedInAllRooms() // <- Allow them in all rooms
                .Run(); //<- Get Crackin
        }
    }
}
