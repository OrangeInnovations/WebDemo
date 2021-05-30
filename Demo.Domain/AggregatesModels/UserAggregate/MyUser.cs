using Common.Entities;
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

        protected MyUser()
        {
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
            MyUserVerifiedDomainEvent myUserVerifiedDomainEvent = new MyUserVerifiedDomainEvent(myUser);
            this.AddDomainEvent(myUserVerifiedDomainEvent);
        }
    }
}
