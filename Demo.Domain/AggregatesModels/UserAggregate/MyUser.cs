using Common.Entities;
using Demo.Domain.AggregatesModels.BlogAggregate;
using Demo.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.AggregatesModels.UserAggregate
{
    public class MyUser : Entity<string>, IAggregateRoot
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedTimeUtc { get; set; }
        public DateTime? UpdatedTimeUtc { get; set; }

        public bool EmailVerified { get; set; }

        public string Name  => FirstName + MiddleName + LastName;

        public List<Blog> MyBlogs { get; set; }
        public List<Post> MyPosts { get; set; }

        protected MyUser()
        {
            MyBlogs = new List<Blog>();
            MyPosts = new List<Post>();

            EmailVerified = false;
            CreatedTimeUtc = DateTime.UtcNow;

        }

        public MyUser(string first, string middle, string last, string email): this()
        {
            FirstName = first;
            MiddleName = middle;
            LastName = last;
            EmailAddress = email;

            AddMyUserVerifiedDomainEvent(this);
        }

        private void AddMyUserVerifiedDomainEvent(MyUser myUser)
        {
            MyUserVerifiedEmailDomainEvent myUserVerifiedDomainEvent = new MyUserVerifiedEmailDomainEvent(myUser);
            this.AddDomainEvent(myUserVerifiedDomainEvent);
        }
    }
}
