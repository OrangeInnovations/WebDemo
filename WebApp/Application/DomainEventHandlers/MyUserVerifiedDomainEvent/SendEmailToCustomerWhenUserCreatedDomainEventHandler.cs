using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

using System.Threading;
using Microsoft.Extensions.Logging;

namespace WebApp.Application.DomainEventHandlers.MyUserVerifiedDomainEvent
{
    public class SendEmailToCustomerWhenUserCreatedDomainEventHandler : INotificationHandler<Demo.Domain.Events.MyUserVerifiedDomainEvent>
    {
        private readonly ILoggerFactory logger;

        public SendEmailToCustomerWhenUserCreatedDomainEventHandler(ILoggerFactory logger)
        {
            this.logger = logger;
        }
        public Task Handle(Demo.Domain.Events.MyUserVerifiedDomainEvent evt, CancellationToken cancellationToken)
        {
            //send email
            logger.CreateLogger<Demo.Domain.Events.MyUserVerifiedDomainEvent>()
               .LogTrace($"{evt.MyUser.Name} has been successfully sent email to {evt.MyUser.EmailAddress})");
            return Task.CompletedTask;
        }


    }
}
