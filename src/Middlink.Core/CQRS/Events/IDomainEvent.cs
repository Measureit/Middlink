using System;

namespace Middlink.Core.CQRS.Events
{
    public interface IDomainEvent : IMessage
    {
    }
    public interface IDomainEvent<TKey> : IDomainEvent
    {
        TKey AggregateId { get; }
    }
}
