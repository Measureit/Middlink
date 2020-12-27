using Middlink.Core.CQRS.Collections;
using System;

namespace Middlink.Core.CQRS.Events
{
    public class CommittedEvent : ICommittedEvent
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public IDomainEvent Data { get; set; }
        public IMetadataCollection Metadata { get; set; }

        public CommittedEvent(Guid aggregateId, int version, IDomainEvent data, IMetadataCollection metadata)
        {
            AggregateId = aggregateId;
            Version = version;
            Data = data;
            Metadata = metadata;
        }
    }
}
