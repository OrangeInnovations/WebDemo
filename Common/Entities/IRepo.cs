using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public interface IRepo<TId, T> where T: Entity<TId>
    {
        Task<T> GetItemAsync(TId id, string partitionKey);

        Task<bool> CreateItemAsync(T item);
        Task<bool> CreateItemAsync(T item, string partitionKey);

        Task<bool> UpdateItemAsync(T item);
        Task<bool> UpdateItemAsync(T item, string partitionKey);
    }
}
