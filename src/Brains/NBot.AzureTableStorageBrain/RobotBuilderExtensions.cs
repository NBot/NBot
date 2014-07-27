using NBot.Core;

namespace NBot.AzureTableStorageBrain
{
    public static class RobotBuilderExtensions
    {
        public static RobotBuilder UseAzureTableStorageBrain(this RobotBuilder builder)
        {
            builder.UseBrain(settings => 
                new AzureTableStorageBrain(settings["AzureTableStorageAccountName"].ToString(), settings["AzureTableStorageAccountKey"].ToString()));
            return builder;
        }

        public static RobotBuilder UseAzureTableStorageBrain(this RobotBuilder builder, string accountName, string accountKey)
        {
            builder.UseBrain(new AzureTableStorageBrain(accountName, accountKey));
            return builder;
        }
    }
}
