
using System;
using System.Collections.Generic;

namespace Middlink.EventSourcing
{
    public class EventDescriptor<T>
    {
        public Guid Id { get; set; }
        public List<T> Events { get; set; }
        public int Version { get; set; }
        public bool IsSent { get; set; } = false;
    }
}
