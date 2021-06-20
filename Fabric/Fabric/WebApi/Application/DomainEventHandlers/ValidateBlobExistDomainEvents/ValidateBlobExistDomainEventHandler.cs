using Demo.Domain.AggregatesModels.BlogAggregate;
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

namespace WebApi.Application.DomainEventHandlers.ValidateBlobExistDomainEvents
{
    public class ValidateBlobExistDomainEventHandler : INotificationHandler<ValidateBlobExistDomainEvent>
    {
        private readonly IBlogRepository repository;
        private readonly ILoggerFactory _logger;
        public ValidateBlobExistDomainEventHandler(IBlogRepository blogRepository, ILoggerFactory logger)
        {
            this.repository = blogRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
       

        public async Task Handle(ValidateBlobExistDomainEvent notification, CancellationToken cancellationToken)
        {
            int blogId = notification.BlogId;
            var blog = await repository.GetAsync(blogId);
            if(blog==null)
            {
                _logger.CreateLogger<ValidateBlobExistDomainEventHandler>()
                   .LogError($"BlobId={blogId} does not exist.");

                var myException = new BlogDomainException($"BlobId={blogId} does not exist.");
                throw myException;
            }
        }
    }
}
