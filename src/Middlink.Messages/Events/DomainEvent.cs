using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Events
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