using Common.Azure.CosmosDb;
using Common.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Azure.Repos
{
    public abstract class CosmosRepo<TId, T> : IRepo<TId, T> where T : Entity<TId>
    {
        public abstract string ContainerId { get; }

        private readonly ICosmosDbClient _cosmosDbClient;
        private readonly IMediator _mediator;

        public CosmosRepo(ICosmosDbClientFactory cosmosDbClientFactory, IMediator mediator)
        {
            _cosmosDbClient = cosmosDbClientFactory.GetClient(ContainerId);
            this._mediator = mediator;
        }

        protected ICosmosDbClient CosmosDbClient => _cosmosDbClient;

        public async Task<T> GetItemAsync(TId id, string partitionKey)
        {
            string sid = id.ToString();

            T response = await _cosmosDbClient.GetItemAsync<T>(sid, partitionKey);

            return response;
        }

        public async Task<bool> CreateItemAsync(T item)
        {
            // Dispatch Domain Events collection.
            await _mediator.DispatchDomainEventsAsync(item);

            var result = await _cosmosDbClient.CreateItemAsync<T>(item);
            if (!string.IsNullOrWhiteSpace(result.Id.ToString()))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateItemAsync(T item, string partitionKey)
        {
            // Dispatch Domain Events collection.
            await _mediator.DispatchDomainEventsAsync(item);

            var result = await _cosmosDbClient.CreateItemAsync<T>(item, partitionKey);
            if (!string.IsNullOrWhiteSpace(result.Id.ToString()))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            // Dispatch Domain Events collection.
            await _mediator.DispatchDomainEventsAsync(item);

            var result = await _cosmosDbClient.UpdateItemAsync<T>(item);
            if (!string.IsNullOrWhiteSpace(result.Id.ToString()))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateItemAsync(T item, string partitionKey)
        {
            // Dispatch Domain Events collection.
            await _mediator.DispatchDomainEventsAsync(item);

            var result = await _cosmosDbClient.UpdateItemAsync<T>(item, partitionKey);
            if (!string.IsNullOrWhiteSpace(result.Id.ToString()))
            {
                return true;
            }
            return false;
        }
    }
}
