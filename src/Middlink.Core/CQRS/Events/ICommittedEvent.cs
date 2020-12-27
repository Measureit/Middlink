using Middlink.Core.CQRS.Collections;
using System;

namespace Middlink.Core.CQRS.Events
{
    public interface ICommittedEvent
    {
        Guid AggregateId { get; }
        int Version { get; }
        IDomainEvent Data { get; }
        IMetadataCollection Metadata { get; }
    }
}
