using System.Collections.Generic;
using System.Linq;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Announce : IRecieveMessages
    {
        private readonly string _roomsConfiguration = Core.NBot.Settings["AnnounceRooms"] as string;
        private readonly List<int> _roomsIds;

        public Announce(IHostAdapter host)
        {
            if (!string.IsNullOrEmpty(_roomsConfiguration))
            {
                _roomsIds = _roomsConfiguration.Contains("*")
                                ? host.GetAllRooms().Select(r => r.Id).ToList()
                                : _roomsConfiguration.Split(new[] {';', ','}).Select(int.Parse).ToList();
            }
        }

        [Help(Syntax = "announce <Message>",
            Description = "The provided messsage will be broadcast to all rooms.",
            Example = "announce Hello Everyone!")]
        [RespondByRegex("announce \"?([^\"]*)\"?")]
        public void RoomAnnounce(IUserMessage message, IHostAdapter host, string[] matches)
        {
            string announcement = matches[1];
            IEntity user = host.GetUser(message.UserId);
            string response = string.Format("{0} says \" {1} \"", user.Name, announcement);
            host.Broadcast(response);
        }
    }
}