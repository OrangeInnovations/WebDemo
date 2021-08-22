using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Common.Entities
{
    public abstract class Entity<TId>: IEntity
    {
        [JsonProperty("id")]
        public virtual TId Id { get; set; }

        public bool IsTransient()
        {
            return Id.Equals(default(TId));
        }

        #region Domain Events

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        #endregion

        #region Override Methods
        public override bool Equals(object entity)
        {

            if ((entity == null) || !(entity is Entity<TId>))
            {
                return false;
            }

            if (object.ReferenceEquals(this, entity))
            {
                return true;
            }

            if (this.GetType() != entity.GetType())
            {
                return false;
            }


            Entity<TId> item = (Entity<TId>)entity;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.Id.Equals(this.Id);
            }

        }

        private int? _requestedHashCode;

        public override int GetHashCode()
        {

            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;// XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity<TId> left,
           Entity<TId> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TId> left,
            Entity<TId> right)
        {
            return (!(left == right));
        }

        #endregion
    }
}
