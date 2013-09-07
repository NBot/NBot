using System.Collections.Generic;
using Autofac;
using NBot.Campfire.Modules;

namespace NBot.Campfire
{
    public static class NBotExtensions
    {
        public static Core.NBot UseCampfire(this Core.NBot nBot, string token, string account, List<int> roomsToJoin)
        {
            nBot.Register(builder =>
                              {
                                  builder.RegisterModule(new CampfireModule());
                              })
                .AddSetting("Token", token)
                .AddSetting("Account", account)
                .AddSetting("RoomsToJoin", roomsToJoin);

            return nBot;
        }
    }
}
