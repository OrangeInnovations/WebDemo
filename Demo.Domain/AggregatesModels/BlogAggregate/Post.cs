using Common.Entities;
using Demo.Domain.Events;
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

        protected Post()
        {
            CreatedTimeUtc = DateTime.UtcNow;
        }

        public Post(string ownerId, string title, string content,int blogId) : this()
        {
            PosterId = ownerId;
            Title = title;
            Content = content;
            BlogId = blogId;

            AddCreatePostDomainEvent(this);
        }

        private void AddCreatePostDomainEvent(Post post)
        {
            ValidateUserExistDomainEvent validateUserExistDomainEvent = new ValidateUserExistDomainEvent(post.PosterId);
            this.AddDomainEvent(validateUserExistDomainEvent);

            ValidateBlobExistDomainEvent validateBlobExistDomainEvent = new ValidateBlobExistDomainEvent(post.BlogId);
            this.AddDomainEvent(validateBlobExistDomainEvent);
        }
    }
}
