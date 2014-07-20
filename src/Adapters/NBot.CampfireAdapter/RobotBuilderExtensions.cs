using System.Linq;
using NBot.Core;

namespace NBot
{
    public static class RobotBuilderExtensions
    {
        public static RobotBuilder UseCampfireAdapter(this RobotBuilder builder, string token, string account, params int[] roomsToJoin)
        {
            builder.RegisterAdapter("Campfire", new CampfireAdapter.CampfireAdapter(token, account, roomsToJoin));

            return builder;
        }

        public static RobotBuilder UseCampfireAdapter(this RobotBuilder builder)
        {
            builder.RegisterAdapter("Campfire", configuration => new CampfireAdapter.CampfireAdapter(
                configuration.Settings["CampfireToken"].ToString(),
                configuration.Settings["CampfireAccount"].ToString(),
                configuration.Settings["CampfireRoomsToJoin"].ToString().Split(new[] { ',', ';' }).Select(int.Parse).ToArray()));
            return builder;
        }
    }
}
