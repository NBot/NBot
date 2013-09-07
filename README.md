NBot
====

A .NET "port" of git hub's Hubot that grew a life of it's own. Hey! .NET folk can have fun too.

## What is NBot?
NBot is a "port" of git hub's Hubot but targeted at the .NET platform. It acts as a messaging framework for you to add on to. Currently there is an implementation for Campfire and a base set of plugins for you to enjoy.

## What can I add to NBot?
1. You can add plugins that respond to messages sent from a message publisher like Campfire. 
2. You can create new message publishers.
3. You can create content filters.
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
 public class HelloNBot : RecieveMessages {

    [RecieveByRegex("Hello NBot")]
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
    Core.NBot.Create("NBot") // The Name of you bot
        .Register(b => b.RegisterModule(new PluginsModule())) // Load the Plugins Module
       .UseCampfire("YOUR_AUTH_CODE", "YOUR_ACCOUNT_SUBDOMAIN", new[] { 12345 }.ToList())
       .UseFileBrain() // Use the File Brain
       .UseHandleBars() // Use Handlebars Brain Data Replacement
       .AddSetting("AnnounceRooms", "*") // This is for the announce plugin
       .Start(); // Get Crackin'
}
```

