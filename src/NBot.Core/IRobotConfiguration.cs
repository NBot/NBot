﻿using System;
using System.Collections.Generic;
using NBot.Core.Brains;
using NBot.Core.Logging;
using NBot.Core.MessageFilters;

namespace NBot.Core
{
    public interface IRobotConfiguration
    {
        IRobotConfiguration AddSetting<T>(string key, T value);
        IRobotConfiguration UseBrain(IBrain brain);
        IRobotConfiguration RegisterAdapter(IAdapter adapter, string recieveChannel);
        IRobotConfiguration RegisterMessageHandler(IMessageHandler handler, params string[] allowedRooms);
        IRobotConfiguration RegisterMessageFilter(IMessageFilter messageFilter);
        IRobotConfiguration UseLog(INBotLog log);
        void Run();
    }
}