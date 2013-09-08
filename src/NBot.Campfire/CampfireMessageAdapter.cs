using System.Collections.Generic;
using NBot.Campfire.Messages;
using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core;
using NBot.Core.Messaging;

namespace NBot.Campfire
{
    public class CampfireMessageAdapter : IMessageAdapter
    {
        private readonly IMessagingService _messagingService;

        public CampfireMessageAdapter(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        #region IMessageAdapter Members

        public void Broadcast(string content)
        {
            foreach (IEntity room in GetAllRooms())
            {
                Send(room.Id, content);
            }
        }

        public void ReplyTo(IMessage message, string content)
        {
            Send(message.RoomId, content);
        }

        public void Send(int roomId, string content)
        {
            _messagingService.Send<UserMessage>(CampfireMessageFactory.CreateSpeakMessage(roomId, content));
        }

        public IEnumerable<IEntity> GetAllRooms()
        {
            return _messagingService.Send<List<Room>>(CampfireMessageFactory.CreateGetAllRoomsMessage());
        }

        public IEnumerable<IEntity> GetPresence()
        {
            return _messagingService.Send<List<Room>>(CampfireMessageFactory.CreateGetPresenceMessage());
        }

        public IEntity GetUser(int userId)
        {
            return _messagingService.Send<User>(CampfireMessageFactory.CreateGetUserMessage(userId));
        }

        public IEnumerable<IEntity> GetUsersInRoom(int roomId)
        {
            return _messagingService.Send<Room>(CampfireMessageFactory.CreateGetRoomMessage(roomId)).Users;
        }

        #endregion
    }
}