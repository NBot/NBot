using System;

namespace NBot.Core
{
    public class MessageHandlerRegistration
    {
        public MessageHandlerRegistration(Func<RobotConfiguration, IMessageHandler> registrationFunction, string[] allowedRooms)
        {
            RegistrationFunction = registrationFunction;
            AllowedRooms = allowedRooms;
        }

        public Func<RobotConfiguration, IMessageHandler> RegistrationFunction { get; set; }
        public string[] AllowedRooms { get; set; }
    }
}