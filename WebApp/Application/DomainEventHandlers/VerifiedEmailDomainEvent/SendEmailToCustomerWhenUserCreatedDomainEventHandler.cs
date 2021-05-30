using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

using System.Threading;
using Microsoft.Extensions.Logging;
using Demo.Domain.Events;

namespace WebApp.Application.DomainEventHandlers.VerifiedEmailDomainEvent
{
    public class SendEmailToCustomerWhenUserCreatedDomainEventHandler : INotificationHandler<MyUserVerifiedEmailDomainEvent>
    {
        private readonly ILoggerFactory logger;

        public SendEmailToCustomerWhenUserCreatedDomainEventHandler(ILoggerFactory logger)
        {
            this.logger = logger;
        }
        public Task Handle(MyUserVerifiedEmailDomainEvent evt, CancellationToken cancellationToken)
        {
            //send email
            logger.CreateLogger<MyUserVerifiedEmailDomainEvent>()
               .LogTrace($"{evt.MyUser.Name} has been successfully sent email to {evt.MyUser.EmailAddress})");
            return Task.CompletedTask;
        }


    }
}
