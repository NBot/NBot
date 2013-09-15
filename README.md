NBot
====

A .NET "port" of git hub's Hubot that grew a life of it's own. Hey! .NET folk can have fun too.

## What is NBot?
NBot is a "port" of git hub's Hubot but targeted at the .NET platform. It acts as a messaging framework for you to add on to. Currently there is an implementation for Campfire and a base set of plugins for you to enjoy.

## What can I add to NBot?
1. You can add Message Handlers that respond to messages sent from an Adapter like Campfire. 
2. You can create new Adapters.
3. You can create Message Filters.
4. Add features to the core application.

## Getting Started
Creating your first plugin.
* Create a class and inherit from RecieveMessages.
* Add a method with an attribute RecieveByRegex.
* The parameters are added as needed and are injected when the types are known.
* Do some logic.
* Reply if you like.
* Done!

```
 public class HelloNBot : MessageHandler {

    [Hear("Hello NBot")]
    public void DoHelloNBot(IUserMessage message, IHostAdapter host){
        IEntity user = host.GetUser(message.UserId);
        host.ReplyTo(message, string.Format("Hi {0}, how are you?",user.Name));
    }
}
```

## Quick Start - Campfire Setup
* Create a console aplication
* Include NBot Core, Campfire and Plugins
* Include Dependencies
* Use the Fluent interface to configure your NBot
* Chat Bot Bliss....


```
static void Main(string[] args)
{
            // New up a brain to use
            var brain = new FileBrain(".\\Brain");

            Robot.Create("NBot")
                .UseBrain(brain) // <- Use your brain
                .RegisterMessageFilter(new HandleBarsMessageFilter(brain)) // <- Register zero or more Message Filters
                .RegisterAdapter(new ConsoleAdapter(), "ConsoleChannel") // <- Register one ore more Adapters
                .RegisterHandler(new Achievement()) // <- Register zero or more message handlers
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
                .Run(); // <- Get Crackin
}
```

