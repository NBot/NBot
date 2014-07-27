using Microsoft.WindowsAzure.Storage.Table;
using ServiceStack;

namespace NBot.AzureTableStorageBrain
{
    public class BrainEntity:TableEntity
    {
        public BrainEntity(string partitionKey, string key, object value)
        {
            PartitionKey = partitionKey;
            RowKey = key;
            Value = value.ToJson();
        }

        public BrainEntity()
        {
            
        }

        public string Value { get; set; }
    }
}
