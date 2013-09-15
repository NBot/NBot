using System;
using System.Linq;
using NBot.Core;
using NBot.Core.Brains;
using NBot.Core.Messaging.ContentFilters;
using NBot.Plugins;
using NBot.CampfireAdapter;

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
                .RegisterHandler(new Achievement())
                .RegisterHandler(new Akbar())
                .RegisterHandler(new Announce())
                .RegisterHandler(new AsciiMe())
                .RegisterHandler(new CalmDown())
                .RegisterHandler(new ChuckNorris())
                .RegisterHandler(new DownForMe())
                .RegisterHandler(new ExcuseMe())
                .RegisterHandler(new FacePalm())
                .RegisterHandler(new FortuneMe())
                .RegisterHandler(new Hello())
                .RegisterHandler(new MemeGenerator())
                .RegisterHandler(new Pager())
                .RegisterHandler(new Ping())
                .RegisterHandler(new Remember())
                .RegisterHandler(new Sensitive())
                .RegisterHandler(new Swanson())
                .Run();
        }
    }
}