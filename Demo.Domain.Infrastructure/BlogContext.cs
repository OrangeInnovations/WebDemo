using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Infrastructure
{
    public class BlogContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "blog";


        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
