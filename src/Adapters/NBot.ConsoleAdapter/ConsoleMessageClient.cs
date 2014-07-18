using System;
using System.Collections.Generic;
using NBot.Core;

namespace NBot.ConsoleAdapter
{
    public class ConsoleMessageClient : IMessageClient
    {
        public void Send(string message, string roomId, string user = null)
        {
            Console.WriteLine(message);
        }

        public void Broadcast(string message)
        {
            Console.WriteLine(message);
        }

        public IEntity GetUser(string userId)
        {
            return new ConsoleEntity(userId,"ConsoleUser");
        }

        public IEntity GetRoom(string roomId)
        {
            return new ConsoleEntity(roomId,"ConsoleRoom");
        }

        public List<IEntity> GetAllRooms()
        {
            return new List<IEntity>() {new ConsoleEntity("ConsoleRoom", "ConsoleRoom")};
        }

        public List<IEntity> GetUsersInRoom(string roomId)
        {
            return new List<IEntity>() { new ConsoleEntity("ConsoleRoom", "ConsoleUser") };
        }
    }
}