using Common.Entities;
using Demo.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Domain.AggregatesModels.BlogAggregate
{
    public class Blog : Entity<int>, IAggregateRoot
    {
        public string OwnerId { get; set; }
        
        public string Url { get; set; }
        public int Rating { get; set; }

        public DateTime CreatedTimeUtc { get; set; }
        public DateTime? UpdatedTimeUtc { get; set; }

        public List<Post> Posts { get; set; }

        protected Blog()
        {
            Posts = new List<Post>();
            Rating = 0;
            CreatedTimeUtc = DateTime.UtcNow;
        }


        public Blog(string ownerId, string url, int rating):this()
        {
            OwnerId = ownerId;
            Url = url;
            Rating = rating;

            AddCreateBlogDomainEvent(this);
        }

        private void AddCreateBlogDomainEvent(Blog blog)
        {
            ValidateUserExistDomainEvent validateUserExistDomainEvent = new ValidateUserExistDomainEvent(blog.OwnerId);
            this.AddDomainEvent(validateUserExistDomainEvent);
        }
    }
}
