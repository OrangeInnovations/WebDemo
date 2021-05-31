using Demo.Domain.AggregatesModels.BlogAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Application.Queries
{
    public interface IBlobQueries
    {
        Task<Blog> GetBlobAsync(int id);
        //Task<>
    }
}
