using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBot.Messaging
{
    public interface IMessageClient
    {
        void SendMessage(Message message);
        IEntity GetUser(string userId);
        IEntity GetRoom(string roomId);
    }
}
