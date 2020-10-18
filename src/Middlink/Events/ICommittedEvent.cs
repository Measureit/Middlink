using Middlink.Collections;
using System;

namespace Middlink.Events
{
    public interface ICommittedEvent
    {
        Guid AggregateId { get; }
        int Version { get; }
        IDomainEvent Data { get; }
        IMetadataCollection Metadata { get; }
    }
}
