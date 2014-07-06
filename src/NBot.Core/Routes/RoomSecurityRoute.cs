﻿using System.Linq;
using System.Reflection;

namespace NBot.Core.Routes
{
    public class RoomSecurityRoute : IRoute, IRoomSecurityRoute
    {
        private readonly string[] _allowedRooms;
        private readonly IRoute _innerRoute;

        public RoomSecurityRoute(IRoute innerRoute, params string[] allowedRooms)
        {
            _innerRoute = innerRoute;
            _allowedRooms = allowedRooms;
        }

        public IRoute InnerRoute
        {
            get { return _innerRoute; }
        }

        public IMessageHandler Handler
        {
            get { return _innerRoute.Handler; }
        }

        public MethodInfo EndPoint
        {
            get { return _innerRoute.EndPoint; }
        }

        public bool IsMatch(Message message)
        {
            return _innerRoute.IsMatch(message) && _allowedRooms.Contains(message.RoomId);
        }
    }
}