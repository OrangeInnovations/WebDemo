using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Common.Entities
{
    public interface IEntity
    {
         IReadOnlyCollection<INotification> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
