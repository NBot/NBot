using NBot.Core;
using NBot.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NBot
{
    public static class RobotBuilderExtenstions
    {
        public static MessageHandlerRoomAuthorization RegisterHandlers(this RobotBuilder robot, params IMessageHandler[] handlers)
        {
            return new MessageHandlerRoomAuthorization(robot, handlers.ToList());
        }

        public static MessageHandlerRoomAuthorization RegisterHandlersInAssembly(this RobotBuilder robot, Assembly assembly)
        {
            return new MessageHandlerRoomAuthorization(robot, GetHandlers(assembly).Select(CreateInstance).Where(h => h != null).ToList());
        }

        public static MessageHandlerRoomAuthorization RegisterHandlersInAssemblyByTags(this RobotBuilder robot, Assembly assembly, params string[] tags)
        {
            return new MessageHandlerRoomAuthorization(robot, GetHandlers(assembly).Where(t => AnyTag(t, tags)).Select(CreateInstance).Where(h => h != null).ToList());
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
