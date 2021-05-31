using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Domain.Events
{
    public class ValidateUserExistDomainEvent : INotification
    {
        public string UserId { get; set; }
        public ValidateUserExistDomainEvent(string userId)
        {
            UserId = userId;
        }
    }
}
