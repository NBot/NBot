using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web.UI;

namespace NBot.Core
{
    public static class RobotExtenstions
    {
        public static HandlerRegistration RegisterHandlers(this IRobotConfiguration robot, params IMessageHandler[] handlers)
        {
            return new HandlerRegistration(robot, handlers.ToList());
        }

        public static HandlerRegistration RegisterHandlersInAssembly(this IRobotConfiguration robot, Assembly assembly)
        {
            var handlers = new List<IMessageHandler>();

            foreach (var handler in assembly.GetTypes().Where(t => typeof(IMessageHandler).IsAssignableFrom(t)))
            {
                var construstor = handler.GetConstructor(Type.EmptyTypes);

                if (construstor != null)
                {
                    var instance = Activator.CreateInstance(handler) as IMessageHandler;
                    handlers.Add(instance);
                }
            }

            return new HandlerRegistration(robot, handlers);
        }

        public class HandlerRegistration
        {
            private readonly IRobotConfiguration _configuration;
            private readonly List<IMessageHandler> _handlers;

            public HandlerRegistration(IRobotConfiguration configuration, List<IMessageHandler> handlers)
            {
                _configuration = configuration;
                _handlers = handlers;
            }

            public IRobotConfiguration AllowedInRooms(params string[] rooms)
            {
                var allowedRooms = rooms.ToList();

                return RegisterHandlers(allowedRooms);
            }

            public IRobotConfiguration AllowedInRooms(params int[] rooms)
            {
                var allowedRooms = rooms.Select(r => r.ToString()).ToArray();
                return AllowedInRooms(allowedRooms);
            }

            public IRobotConfiguration AllowedInAllRooms()
            {
                return RegisterHandlers(null);
            }

            private IRobotConfiguration RegisterHandlers(List<string> allowedRooms)
            {
                foreach (var handler in _handlers)
                {
                    _configuration.RegisterHandler(handler, allowedRooms);
                }

                return _configuration;
            }
        }
    }
}
