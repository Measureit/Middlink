using System;

namespace Middlink.Core.CQRS.Events
{
    public interface IDomainEvent : IMessage
    {
        Guid AggregateId { get; }
    }
}
