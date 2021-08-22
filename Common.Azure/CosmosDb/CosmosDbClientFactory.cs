using System;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
namespace Common.Azure.CosmosDb
{
    public class CosmosDbClientFactory : ICosmosDbClientFactory
    {
        private static readonly Dictionary<string, ICosmosDbClient> _cosmosDbClients = new Dictionary<string, ICosmosDbClient>();

        private static object synclock = new object();

        private readonly string _databaseId;
        private readonly CosmosClient _cosmosClient;

        public CosmosDbClientFactory(string databaseId, CosmosClient cosmosClient)
        {
            this._databaseId = databaseId;
            this._cosmosClient = cosmosClient;
        }

        public ICosmosDbClient GetClient(string containerId)
        {
            ICosmosDbClient cosmosDbClient;
            lock(synclock)
            {
                if(!_cosmosDbClients.TryGetValue(containerId, out cosmosDbClient))
                {
                    cosmosDbClient = new CosmosDbClient(_databaseId, containerId, _cosmosClient);
                    _cosmosDbClients.Add(containerId, cosmosDbClient);
                }
            }
            return cosmosDbClient;
        }
    }
}
