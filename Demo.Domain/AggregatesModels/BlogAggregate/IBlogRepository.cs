using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.AggregatesModels.BlogAggregate
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Blog Add(Blog blog);
        void Update(Blog blog);

        Task<Blog> GetAsync(int blogId);
    }
}
