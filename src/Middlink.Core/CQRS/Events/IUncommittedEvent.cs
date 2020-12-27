using Middlink.Core.CQRS.Collections;
using System;

namespace Middlink.Core.CQRS.Events
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
