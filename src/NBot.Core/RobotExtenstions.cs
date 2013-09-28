using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using NBot.Core.Attributes;
using ServiceStack.Common.Extensions;

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
            return new HandlerRegistration(robot, GetHandlers(assembly).Select(CreateInstance).Where(h => h != null).ToList());
        }

        public static HandlerRegistration RegisterHandlersInAssemblyByTags(this IRobotConfiguration robot, Assembly assembly, params string[] tags)
        {
            return new HandlerRegistration(robot, GetHandlers(assembly).Where(t => AnyTag(t, tags)).Select(CreateInstance).Where(h => h != null).ToList());
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

        private static IEnumerable<Type> GetHandlers(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(IMessageHandler).IsAssignableFrom(t));
        }

        private static bool AnyTag(Type handlerType, IEnumerable<string> tags)
        {
            var tagAttributes = handlerType.GetCustomAttributes(typeof(TagAttribute), true);

            return tagAttributes.Cast<TagAttribute>().Any(tagAttribute => tagAttribute.Tags.Any(tag => tags.Select(t => t.ToLower()).Any(t => t == tag)));
        }

        private static IMessageHandler CreateInstance(Type handlerType)
        {
            var construstor = handlerType.GetConstructor(Type.EmptyTypes);

            if (construstor != null)
            {
                return Activator.CreateInstance(handlerType) as IMessageHandler;
            }

            return null;
        }
    }
}
