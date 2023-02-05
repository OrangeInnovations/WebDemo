using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Events;
using Demo.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Services.DomainEventHandlers.ValidateUserExistDomainEvents
{
    public class ValidateUserExistDomainEventHandler : INotificationHandler<ValidateUserExistDomainEvent>
    {
        private readonly IMyUserRepository myUserRepository;
        private readonly ILoggerFactory _logger;
        public ValidateUserExistDomainEventHandler(IMyUserRepository myUserRepository, ILoggerFactory logger)
        {
            this.myUserRepository = myUserRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(ValidateUserExistDomainEvent notification, CancellationToken cancellationToken)
        {
            string userId = notification.UserId;

            var user=await myUserRepository.GetAsync(userId);

            if(user==null)
            {
                _logger.CreateLogger<ValidateUserExistDomainEventHandler>()
                    .LogError($"UserId={userId} does not exist.");

               var myException = new BlogDomainException($"UserId={userId} does not exist.");
                throw myException;
            }
            
        }
    }
}
