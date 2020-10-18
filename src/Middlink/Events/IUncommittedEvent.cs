using Middlink.Collections;
using System;

namespace Middlink.Events
{
    public interface IUncommittedEvent
    {
        Guid AggregateId { get; }
        DateTime CreatedAt { get; }
        int Version { get; }
        IDomainEvent Data { get; }
        IMetadataCollection Metadata { get; }
    }
}
