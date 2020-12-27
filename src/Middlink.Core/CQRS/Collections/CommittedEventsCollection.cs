using Middlink.Core.CQRS.Events;
using System.Collections.Generic;

namespace Middlink.Core.CQRS.Collections
{
    public class CommittedEventsCollection : HashSet<IDomainEvent>
    {
        public CommittedEventsCollection(IEnumerable<IDomainEvent> events) : base(events)
        {
        }
    }
}
