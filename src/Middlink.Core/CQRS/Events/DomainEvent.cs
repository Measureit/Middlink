using System;

namespace Middlink.Core.CQRS.Events
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
