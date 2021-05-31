using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Events
{
    public class ValidateBlobExistDomainEvent : INotification
    {
        public int BlogId { get; set; }
        public ValidateBlobExistDomainEvent(int blogId)
        {
            BlogId = blogId;
        }
    }
}
