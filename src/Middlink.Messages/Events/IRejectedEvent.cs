using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Events
{
    public interface IRejectedEvent : IDomainEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}
