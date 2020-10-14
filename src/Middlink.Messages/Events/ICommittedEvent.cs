using Middlink.Messages.Collections;
using System;

namespace Middlink.Messages.Events
{
    public interface ICommittedEvent
    {
        Guid AggregateId { get; }
        int Version { get; }
        IDomainEvent Data { get; }
        IMetadataCollection Metadata { get; }
    }
}
