using System;

namespace Middlink.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; }

        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}
