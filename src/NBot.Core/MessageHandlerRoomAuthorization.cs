using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NBot.Core
{
    public class MessageHandlerRoomAuthorization
    {
        private readonly RobotBuilder _robotBuilder;
        private readonly List<IMessageHandler> _handlers;

        public MessageHandlerRoomAuthorization(RobotBuilder robotBuilder, List<IMessageHandler> handlers)
        {
            _robotBuilder = robotBuilder;
            _handlers = handlers;
        }

        public RobotBuilder AllowedInRooms(params string[] rooms)
        {
            return RegisterHandlers(rooms);
        }

        public RobotBuilder AllowedInRooms(params int[] rooms)
        {
            var allowedRooms = rooms.Select(r => r.ToString(CultureInfo.InvariantCulture)).ToArray();
            return AllowedInRooms(allowedRooms);
        }

        public RobotBuilder AllowedInAllRooms()
        {
            return RegisterHandlers(null);
        }

        private RobotBuilder RegisterHandlers(params string[] allowedRooms)
        {
            foreach (var handler in _handlers)
            {
                _robotBuilder.RegisterMessageHandler(handler, allowedRooms);
            }

            return _robotBuilder;
        }
    }
}