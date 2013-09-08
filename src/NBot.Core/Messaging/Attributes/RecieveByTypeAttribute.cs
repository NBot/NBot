﻿using System;
using System.Reflection;
using NBot.Core.Messaging.Routes;

namespace NBot.Core.Messaging.Attributes
{
    public class RecieveByTypeAttribute : RecieveMessageAttribute
    {
        private readonly string _messageType;

        public RecieveByTypeAttribute(string messageType)
        {
            _messageType = messageType;
        }

        public override IRoute CreateRoute(Type reciever, MethodInfo endpoint)
        {
            return new MessageTypeRoute(reciever, endpoint, _messageType);
        }
    }
}