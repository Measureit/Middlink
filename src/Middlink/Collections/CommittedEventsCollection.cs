using Middlink.Events;
using System.Collections.Generic;

namespace Middlink.Collections
{
    public class CommittedEventsCollection : HashSet<IDomainEvent>
    {
        public CommittedEventsCollection(IEnumerable<IDomainEvent> events) : base(events)
        {
        }
    }
}
