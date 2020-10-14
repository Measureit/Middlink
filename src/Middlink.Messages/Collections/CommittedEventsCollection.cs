using Middlink.Messages.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Collections
{
    public class CommittedEventsCollection : HashSet<IDomainEvent>
    {
        public CommittedEventsCollection(IEnumerable<IDomainEvent> events) : base(events)
        {
        }
    }
}
