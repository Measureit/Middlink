using System;

namespace Middlink.Core.CQRS.Events
{
    public abstract class DomainEvent<TKey> : IDomainEvent<TKey>
    {
        public TKey AggregateId { get; }

        protected DomainEvent(TKey aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
