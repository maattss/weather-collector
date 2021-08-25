using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace WeatherCollector.Repositories
{
    public abstract class BaseRepository<T> where T : TableEntity, new()
    {
        private readonly string connectionString;
        private readonly string tableName;

        public BaseRepository(string tableName, string connectionString)
        {
            this.tableName = tableName;
            this.connectionString = connectionString;
        }
        public CloudTable GetTable()
        {
            try
            {
                var client = GetCloudTableClient();
                var table = client.GetTableReference(tableName);
                return table;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<CloudTable> AddOrGetTableAsync()
        {
            try
            {
                var client = GetCloudTableClient();
                var table = client.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync();
                return table;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task DeleteTable()
        {
            try
            {
                CloudTable table = GetTable();
                await table.DeleteIfExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        protected CloudStorageAccount GetStorageAccount()
        {
            try
            {
                return CloudStorageAccount.Parse(connectionString);
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        protected async Task<T> AddOrUpdateAsync(T entity)
        {
            try
            {
                var insertOperation = TableOperation.InsertOrMerge(entity);
                var table = await AddOrGetTableAsync();

                await table.ExecuteAsync(insertOperation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return entity;
        }

        protected async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var table = await AddOrGetTableAsync();

                var result = await table.ExecuteAsync(retrieveOperation);

                return (T)result.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        protected async Task<IEnumerable<T>> GetByPartitionKeyAsync(string partitionKey)
        {
            try
            {
                TableQuery<T> partitionScanQuery =
                    new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
                TableContinuationToken token = null;
                var table = await AddOrGetTableAsync();
                ICollection<T> results = new List<T>();
                do
                {
                    TableQuerySegment<T> segment = await table.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                    token = segment.ContinuationToken;
                    foreach (T entity in segment)
                    {
                        results.Add(entity);
                    }
                }
                while (token != null);
                return results;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        protected async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                TableQuery<T> tableQuery = new TableQuery<T>();
                TableContinuationToken token = null;
                var table = await AddOrGetTableAsync();
                ICollection<T> results = new List<T>();
                do
                {
                    TableQuerySegment<T> segment = await table.ExecuteQuerySegmentedAsync(tableQuery, token);
                    foreach (T entity in segment)
                    {
                        results.Add(entity);
                    }
                    token = segment.ContinuationToken;
                }
                while (token != null);
                return results;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        protected async Task<IEnumerable<T>> GetByRowKeyAsync(string rowKey)
        {
            try
            {
                TableQuery<T> partitionScanQuery =
                    new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
                TableContinuationToken token = null;
                var table = await AddOrGetTableAsync();
                ICollection<T> results = new List<T>();
                do
                {
                    TableQuerySegment<T> segment = await table.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                    token = segment.ContinuationToken;
                    foreach (T entity in segment)
                    {
                        results.Add(entity);
                    }
                }
                while (token != null);
                return results;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        protected async Task DeleteAsync(T entity)
        {
            try
            {
                var deleteOperation = TableOperation.Delete(entity);
                var table = GetTable();
                await table.ExecuteAsync(deleteOperation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        protected CloudTableClient GetCloudTableClient()
        {
            try
            {

                var storageAccount = GetStorageAccount();
                return storageAccount.CreateCloudTableClient();
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
