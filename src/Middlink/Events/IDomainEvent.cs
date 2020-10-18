using System;

namespace Middlink.Events
{
    public interface IDomainEvent : IMessage
    {
        Guid AggregateId { get; }
    }
}
