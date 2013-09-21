using System;
using System.Collections.Generic;
using NBot.Core.Brains;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public interface IRobotConfiguration
    {
        IRobotConfiguration AddSetting<T>(string key, T value);
        IRobotConfiguration UseBrain(IBrain brain);
        IRobotConfiguration RegisterAdapter(IAdapter adapter, string recieveChannel);
        IRobotConfiguration RegisterHandler(IMessageHandler handler, List<string> allowedRooms = null);
        IRobotConfiguration RegisterMessageFilter(IMessageFilter messageFilter);
        void Run();
    }
}