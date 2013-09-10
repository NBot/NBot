using System;
using System.Collections.Generic;
using NBot.Campfire.Messages;
using NBot.Campfire.Messages.IncomingMessages;
using NBot.Core;
using NBot.Core.Messaging;

namespace NBot.Campfire
{
    public class Room : IEntity
    {
        public List<User> Users;
        public string Topic { get; set; }
        public int MembershipLimit { get; set; }
        public bool Full { get; set; }
        public bool OpenToGuests { get; set; }
        public string ActiveTokenValue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public IMessagingService MessagingService { get; set; }

        #region IEntity Members

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion

        public UserMessage Speak(string message)
        {
            return MessagingService.Send<UserMessage>(CampfireMessageFactory.CreateSpeakMessage(Id, message));
        }

        public bool ChangeTopic(string topic)
        {
            return MessagingService.Send<bool>(CampfireMessageFactory.CreateChangeTopicMessage(Id, topic));
        }

        public bool ChangeName(string name)
        {
            return MessagingService.Send<bool>(CampfireMessageFactory.CreateChangeNameMessage(Id, name));
        }

        public bool Join()
        {
            return MessagingService.Send<bool>(CampfireMessageFactory.CreateJoinRoomMessage(Id));
        }

        public bool Leave()
        {
            return MessagingService.Send<bool>(CampfireMessageFactory.CreateLeaveRoomMessage(Id));
        }

        public bool Lock()
        {
            return MessagingService.Send<bool>(CampfireMessageFactory.CreateLockRoomMessage(Id));
        }

        public bool Unlock()
        {
            return MessagingService.Send<bool>(CampfireMessageFactory.CreateUnlockRoomMessage(Id));
        }
    }
}