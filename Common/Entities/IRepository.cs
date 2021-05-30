using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
