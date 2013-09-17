using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NBot.Core;
using NBot.Core.Attributes;
using NBot.Core.Help;

namespace NBot.MessageHandlers
{
    public class Pager : MessageHandler
    {
        [Help(Syntax = "page <User>",
            Description = "This command will search all rooms for the specified user and let them know you need to speak with them.",
            Example = "page Jon")]
        [Respond("page {{name}}")]
        public void PageUser(Message message, IMessageClient client, string name)
        {
            string regex = string.Format("(.*)({0})(.*)", name);
            List<IEntity> rooms = client.GetAllRooms().ToList();

            string sentFrom = client.GetUser(message.UserId).Name;
            IEntity requestedRoom = rooms.Single(r => r.Id == message.RoomId);
            const string pageMessage = "Paging {0} ... Paging {0} ... Your presence is requested by {1} in the \"{2}\" room.";
            const string failureMessage = "Sorry, nobody by the name {0} could be found.";
            const string userIsInYourRoom = "Lulz!! {0} is in your room.";
            var roomsUserIsIn = new List<IEntity>();

            bool hasMatch = false;
            foreach (IEntity room in rooms)
            {
                IEnumerable<IEntity> usersInRoom = client.GetUsersInRoom(room.Id);
                foreach (IEntity user in usersInRoom)
                {
                    if (Regex.IsMatch(user.Name, regex, RegexOptions.IgnoreCase))
                    {
                        roomsUserIsIn.Add(room);
                        string response = string.Format(pageMessage, user.Name, sentFrom, requestedRoom.Name);

                        if (room.Id == requestedRoom.Id)
                            response = string.Format(userIsInYourRoom, user.Name);

                        client.Send(response, room.Id);
                        hasMatch = true;
                    }
                }
            }

            if (!hasMatch)
            {
                string response = string.Format(failureMessage, name);
                client.Send(response, requestedRoom.Id);
            }
            else
            {
                var responseMessage = new StringBuilder();
                responseMessage.AppendLine(string.Format("{0}, \"{1}\" was found in the following rooms:", sentFrom, name));

                foreach (IEntity room in roomsUserIsIn)
                {
                    responseMessage.AppendLine(string.Format("- {0}", room.Name));
                }

                client.Send(responseMessage.ToString(), requestedRoom.Id);
            }
        }
    }
}