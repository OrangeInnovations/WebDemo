using Common.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Azure
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, IEntity entity)
        {
            var domainEvents = entity.DomainEvents;

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

            entity.ClearDomainEvents();
        }
    }
}
