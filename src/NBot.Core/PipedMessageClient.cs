using System.Collections.Generic;
using ServiceStack.Text;

namespace NBot.Core
{
    /// <summary>
    /// This client will all a passthrough of everything that is not a send or a broadcast. 
    /// Instead it is captured so it can be piped into the next command
    /// </summary>
    public class PipedMessageClient : IMessageClient
    {
        private readonly IMessageClient _innerClient;
        private readonly SortedSet<string> _output = new SortedSet<string>();

        public string[] Output { get { return _output.ToArray(); } }

        public PipedMessageClient(IMessageClient innerClient)
        {
            _innerClient = innerClient;
        }

        public void Send(string message, string roomId, string user = null)
        {
            _output.Add(message);
        }

        public void Broadcast(string message)
        {
            _output.Add(message);
        }

        public IEntity GetUser(string userId)
        {
            return _innerClient.GetUser(userId);
        }

        public IEntity GetRoom(string roomId)
        {
            return _innerClient.GetRoom(roomId);
        }

        public List<IEntity> GetAllRooms()
        {
            return _innerClient.GetAllRooms();
        }

        public List<IEntity> GetUsersInRoom(string roomId)
        {
            return _innerClient.GetUsersInRoom(roomId);
        }

        public string ReplaceInput(string message)
        {
            return message.FormatWith(_output.ToArray());
        }
    }
}
