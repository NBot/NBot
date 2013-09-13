using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public interface IMessageClient
    {
        void SendMessage(string message, string roomId, string userId = null);
        IUser GetUser(string userId);
        IRoom GetRoom(string roomId);
    }
}
