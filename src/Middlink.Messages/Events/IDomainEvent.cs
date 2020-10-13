using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Events
{
    public interface IDomainEvent : IMessage
    {
        Guid AggregateId { get; }
    }
}
