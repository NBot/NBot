using System.Reflection;
using NBot.Core;
using NBot.Core.Brains;
using NBot.Core.Messaging.ContentFilters;

namespace NBot.ConsoleAdapter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var brain = new FileBrain(".\\Brain");
            Robot.Create("NBot")
                .UseBrain(brain)
                .RegisterMessageFilter(new HandleBarsMessageFilter(brain))
                .RegisterAdapter(new ConsoleAdapter(), "ConsoleChannel")
                .RegisterHandlersInAssembly(Assembly.Load("NBot.MessageHandlers"))
                .AllowedInAllRooms()
                .Run();

        }
    }
}