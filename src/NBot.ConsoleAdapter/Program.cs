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
            // New up a brain to use
            var brain = new FileBrain(".\\Brain");

            Robot.Create("NBot")
                .UseBrain(brain) // <- Use your brain
                .RegisterMessageFilter(new HandleBarsMessageFilter(brain)) // <- Register zero or more Message Filters
                .RegisterAdapter(new ConsoleAdapter(), "ConsoleChannel") // <- Register one ore more Adapters
                .RegisterHandlersInAssembly(Assembly.Load("NBot.MessageHandlers")) // <- Register all the Handlers
                .AllowedInAllRooms() // <- Allow them in all rooms
                .Run(); // <- Get Crackin
        }
    }
}