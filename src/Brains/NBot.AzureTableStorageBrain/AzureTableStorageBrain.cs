using NBot.Core.Brains;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ServiceStack;

namespace NBot.AzureTableStorageBrain
{
    public class AzureTableStorageBrain : IBrain
    {
        private readonly string _partitionKey;
        private readonly CloudTable _table;
        private const string NbotTableName = "NBot";

        public AzureTableStorageBrain(string accountName, string accountKey, string partitionKey = "Brain")
            : this(string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey), partitionKey)
        {

        }

        public AzureTableStorageBrain(string connectionString, string partitionKey)
        {
            _partitionKey = partitionKey;
            CloudStorageAccount storrageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storrageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(NbotTableName);
            _table.CreateIfNotExists();
        }

        public object GetValue(string key)
        {
            var result = GetAzureTableResult(key).Result as BrainEntity;
            return result != null ? result.Value.FromJson<object>() : null;
        }

        public void SetValue(string key, object value)
        {
            var tableResult = GetAzureTableResult(key);
            var entity = tableResult.Result as BrainEntity;

            if (entity != null)
            {
                entity.Value = value.ToJson();
                var operation = TableOperation.InsertOrReplace(entity);
                _table.Execute(operation);
            }
            else
            {
                var operation = TableOperation.Insert(new BrainEntity(_partitionKey, key, value));
                _table.Execute(operation);
            }
        }

        public bool ContainsKey(string key)
        {
            return GetAzureTableResult(key).Result != null;
        }

        public void RemoveKey(string key)
        {
            var result = GetAzureTableResult(key).Result as BrainEntity;
            if (result != null)
            {
                var operation = TableOperation.Delete(result);
                _table.Execute(operation);
            }
        }

        private TableResult GetAzureTableResult(string key)
        {
            var operation = TableOperation.Retrieve<BrainEntity>(_partitionKey, key);
            var result = _table.Execute(operation);
            return result;
        }
    }
}
