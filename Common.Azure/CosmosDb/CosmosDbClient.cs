using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;

namespace Common.Azure.CosmosDb
{
    public class CosmosDbClient : ICosmosDbClient
    {
        private readonly Container _container;
        private readonly CosmosClient _cosmosClient;
        private readonly JsonSerializer Serializer = new JsonSerializer();

        public CosmosDbClient(string databaseId, string containerId, CosmosClient cosmosClient)
        {
            this._cosmosClient = cosmosClient;
            _container = cosmosClient.GetContainer(databaseId, containerId);
        }

        public async Task<T> CreateItemAsync<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var response = await _container.CreateItemAsync(item).ConfigureAwait(false);

            return response.Resource;
        }

        public async Task<T> CreateItemAsync<T>(T item, string partitionKey)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            var response = await _container.CreateItemAsync(item, new PartitionKey(partitionKey)).ConfigureAwait(false);

            return response.Resource;
        }

        public async Task<T> GetItemAsync<T>(string id, string partitionKey)
        {
            double requestCharge = 0;
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey)).ConfigureAwait(false);
                requestCharge = response.RequestCharge;
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(T);
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(IQueryable<T> query)
        {
            List<T> results = new List<T>();
            double requestCharge = 0;
            int batchCount = 0;
            using (var feed = query.ToFeedIterator())
            {
                while (feed.HasMoreResults)
                {
                    var response = await feed.ReadNextAsync().ConfigureAwait(false);
                    requestCharge += response.RequestCharge;
                    results.AddRange(response.ToList());
                    batchCount++;
                }
            }
            return results;
        }

        public IQueryable<T> GetItemsQueryable<T>()
        {
            return _container.GetItemLinqQueryable<T>();
        }

        public async Task<T> UpdateItemAsync<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            //var response= await _container.user

            var response = await _container.UpsertItemAsync(item).ConfigureAwait(false);

            return response.Resource;
        }

        public async Task<T> UpdateItemAsync<T>(T item, string partitionKey)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            //var response= await _container.user
            using (Stream stream = ToStream<T>(item))
            {
                using (ResponseMessage responseMessage = await _container.CreateItemStreamAsync(stream, new PartitionKey(partitionKey)).ConfigureAwait(false))
                {
                    T streamResponse = FromStream<T>(responseMessage.Content);
                    return streamResponse;
                }
            }
            throw new NotImplementedException();
        }

        private Stream ToStream<T>(T input)
        {
            MemoryStream streamPayload = new MemoryStream();
            using (StreamWriter streamWriter = new StreamWriter(streamPayload, encoding: Encoding.Default, bufferSize: 1024, leaveOpen: true))
            {
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    writer.Formatting = Newtonsoft.Json.Formatting.None;
                    Serializer.Serialize(writer, input);
                    writer.Flush();
                    streamWriter.Flush();
                }
            }

            streamPayload.Position = 0;
            return streamPayload;
        }

        private T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                using (StreamReader sr = new StreamReader(stream))
                {
                    using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
                    {
                        return Serializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }
        }
    }
}
