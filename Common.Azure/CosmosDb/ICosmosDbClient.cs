using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Common.Azure.CosmosDb
{
    public interface ICosmosDbClient
    {
        IQueryable<T> GetItemsQueryable<T>();
        Task<IEnumerable<T>> GetItemsAsync<T>(IQueryable<T> query);
        Task<T> GetItemAsync<T>(string id, string partitionKey);
        Task<T> CreateItemAsync<T>(T item);
        Task<T> CreateItemAsync<T>(T item, string partitionKey);
        Task<T> UpdateItemAsync<T>(T item);
        Task<T> UpdateItemAsync<T>(T item, string partitionKey);
    }
}
