using Middlink.Messages.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Events
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
