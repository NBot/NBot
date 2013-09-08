using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core.Messaging;

namespace NBot.Campfire.Messages
{
    public static class CampfireMessageFactory
    {
        public static GenericMessage CreateGenericMessage(int roomId, string content, string messageType)
        {
            return new GenericMessage() { RoomId = roomId, Content = content, MessageType = messageType };
        }

        public static UserMessage CreateUserMessage(int userId, int roomId, string content, string messageType)
        {
            return new UserMessage() { UserId = userId, RoomId = roomId, Content = content, MessageType = messageType };
        }

        public static GenericMessage CreateChangeNameMessage(int roomId, string name)
        {
            return CreateGenericMessage(roomId, name, "CampfireChangeNameMessage");
        }

        public static GenericMessage CreateChangeTopicMessage(int roomId, string topic)
        {
            return CreateGenericMessage(roomId, topic, "CampfireChangeTopicMessage");
        }

        public static GenericMessage CreateGetAccountMessage()
        {
            return CreateGenericMessage(0, string.Empty, "CampfireGetAccountMessage");
        }

        public static GenericMessage CreateGetMyUserMessage()
        {
            return CreateGenericMessage(0, string.Empty, "CampfireGetMyUserMessage");
        }

        public static GenericMessage CreateGetAllRoomsMessage()
        {
            return CreateGenericMessage(0, string.Empty, "CampfireGetAllRoomsMessage");
        }

        public static GenericMessage CreateGetPresenceMessage()
        {
            return CreateGenericMessage(0, string.Empty, "CampfireGetPresenceMessage");
        }

        public static GenericMessage CreateGetRoomMessage(int roomId)
        {
            return CreateGenericMessage(roomId, string.Empty, "CampfireGetRoomMessage");
        }

        public static UserMessage CreateGetUserMessage(int userId)
        {
            return CreateUserMessage(userId, 0, string.Empty, "CampfireGetUserMessage");
        }

        public static GenericMessage CreateJoinRoomMessage(int roomId)
        {
            return CreateGenericMessage(roomId, string.Empty, "CampfireJoinRoomMessage");
        }

        public static GenericMessage CreateLeaveRoomMessage(int roomId)
        {
            return CreateGenericMessage(roomId, string.Empty, "CampfireLeaveRoomMessage");
        }

        public static GenericMessage CreateLockRoomMessage(int roomId)
        {
            return CreateGenericMessage(roomId, string.Empty, "CampfireLockRoomMessage");
        }

        public static GenericMessage CreateUnlockRoomMessage(int roomId)
        {
            return CreateGenericMessage(roomId, string.Empty, "CampfireUnlockRoomMessage");
        }

        public static GenericMessage CreateSpeakMessage(int roomId, string message)
        {
            return CreateGenericMessage(roomId, message, "CampfireSpeakMessage");
        }
    }
}
