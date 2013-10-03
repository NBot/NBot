NBot
====
I made this better!!!!
## What is NBot?
NBot is a "port" of git hub's Hubot but targeted at the .NET platform. It acts as a messaging framework for you to add on to. Currently there is an implementation for Campfire and a base set of plugins for you to enjoy.

## What can I add to NBot?
1. You can add Message Handlers that respond to messages sent from an Adapter like Campfire. 
2. You can create new Adapters.
3. You can create Message Filters.
4. Add features to the core application.

## Want to contribute?
1. Create wiki pages
2. Create [Message Handlers](https://github.com/NBot/NBot/wiki/Message-Handler)
3. Create [Adapters](https://github.com/NBot/NBot/wiki/Adapter)
4. Create [Message Filters](https://github.com/NBot/NBot/wiki/MessageFilter)

## Quick Start - Console Setup
* Set ConsoleAdapter Project as the Startup Project
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
        .RegisterHandlersInAssembly(Assembly.Load("NBot.MessageHandlers")) // <- Register all the Handlers
        .AllowedInAllRooms() // <- Allow them in all rooms
        .Run(); // <- Get Crackin
}
```
See [Configration](https://github.com/NBot/NBot/wiki/Configuration) for more details.

## Quick Start - Campfire 
* Create a new Console Application
* Refreence NBot.Core, NBot.MessageHandlers, NBot.CampfireAdapter
* Use the Fluent interface to configure your NBot
* Chat Bot Nirvana..

```
static void Main(string[] args)
{
    // New up a brain to use
    var brain = new SimpleBrain();
    IAdapter adapter = new CampfireAdapter.CampfireAdapter("TOKEN", "SUBDOMAIN", new List<int>() { ROOMS_TO_JOIN });

    Robot.Create("NAME", "ALIAS", "ENVIRONMENT")
        .UseBrain(brain) // <- Use your brain
        .RegisterMessageFilter(new HandleBarsMessageFilter(brain)) // <- Register zero or more Message Filters
        .RegisterAdapter(adapter, "Campfire") // <- Register one ore more Adapters
        .RegisterHandlersInAssembly(Assembly.Load("NBot.MessageHandlers")) // <- Register all the Handlers
        .AllowedInAllRooms() // <- Allow them in all rooms
        .Run(); // <- Get Crackin
}

```

See [Configration](https://github.com/NBot/NBot/wiki/Configuration) for more details.

## Getting Started
Creating your first plugin.
* Create a class and inherit from MessageHandler.
* Add a method with an attribute "Hear".
* The parameters are added as needed and are injected when the types are known.
* Do some logic.
* Reply if you like.
* Done!

```
 public class HelloNBot : MessageHandler {

    [Hear("Hello NBot")]
    public void HandleHello(Message message, IMessagingClient client){
        IEntity user = client.GetUser(message.UserId);
        client.ReplyTo(message, string.Format("Hi {0}, how are you?",user.Name));
    }
}
```
see [Message Handler](https://github.com/NBot/NBot/wiki/Message-Handler) for more information...

## Contributors
1. [Jonathan Starnes](https://github.com/JonathanStarnes) - Project Lead
2. [Billy Laney](https://github.com/supabill)
3. [Tyler Falkenhgen](https://github.com/tfalkenhagen)


## Open Source Dependencies
1. [Top Shelf](https://github.com/phatboyg/Topshelf)
2. [ServiceStack.Common](https://github.com/ServiceStack/ServiceStack)
2. [ServiceStack.Text](https://github.com/ServiceStack/ServiceStack.Text)
3. [ServiceStack.Interfaces](https://github.com/ServiceStack/ServiceStack)

The NBot Logo was provided by The Noun Project
http://thenounproject.com/



