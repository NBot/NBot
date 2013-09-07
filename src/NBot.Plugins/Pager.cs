using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Help;
using NBot.Core.Messaging;
using NBot.Core.Messaging.Attributes;

namespace NBot.Plugins
{
    public class Pager : RecieveMessages
    {
        [Help(Syntax = "page <User>",
            Description = "This command will search all rooms for the specified user and let them know you need to speak with them.",
            Example = "page Jon")]
        [RespondByRegex("page (.*)")]
        public void PageUser(IUserMessage message, IHostAdapter host, string[] matches)
        {
            string name = matches[1];
            string regex = string.Format("(.*)({0})(.*)", matches[1]);
            List<IEntity> rooms = host.GetAllRooms().ToList();

            string sentFrom = host.GetUser(message.UserId).Name;
            IEntity requestedRoom = rooms.Single(r => r.Id == message.RoomId);
            const string pageMessage = "Paging {0} ... Paging {0} ... Your presence is requested by {1} in the \"{2}\" room.";
            const string failureMessage = "Sorry, nobody by the name {0} could be found.";
            const string userIsInYourRoom = "Lulz!! {0} is in your room.";
            var roomsUserIsIn = new List<IEntity>();

            bool hasMatch = false;
            foreach (IEntity room in rooms)
            {
                IEnumerable<IEntity> usersInRoom = host.GetUsersInRoom(room.Id);
                foreach (IEntity user in usersInRoom)
                {
                    if (Regex.IsMatch(user.Name, regex, RegexOptions.IgnoreCase))
                    {
                        roomsUserIsIn.Add(room);
                        string response = string.Format(pageMessage, user.Name, sentFrom, requestedRoom.Name);

                        if (room.Id == requestedRoom.Id)
                            response = string.Format(userIsInYourRoom, user.Name);

                        host.Send(room.Id, response);
                        hasMatch = true;
                    }
                }
            }

            if (!hasMatch)
            {
                string response = string.Format(failureMessage, name);
                host.Send(requestedRoom.Id, response);
            }
            else
            {
                var responseMessage = new StringBuilder();
                responseMessage.AppendLine(string.Format("{0}, \"{1}\" was found in the following rooms:", sentFrom, name));

                foreach (IEntity room in roomsUserIsIn)
                {
                    responseMessage.AppendLine(string.Format("- {0}", room.Name));
                }

                host.Send(requestedRoom.Id, responseMessage.ToString());
            }
        }
    }
}