using Common.Entities;
using Demo.Domain.Exceptions;
using System;

namespace Demo.Domain.AggregatesModels.BlogAggregate
{
    public class Post : Entity<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string PosterId { get; set; }

        public DateTime CreatedTimeUtc { get; set; }
        public DateTime? UpdatedTimeUtc { get; set; }


        public int BlogId { get; set; }
        public Blog Blog { get; set; }


    }
}
