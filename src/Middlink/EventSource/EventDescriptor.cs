using Middlink.Messages.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.EventSource
{
    public class EventDescriptor<T> : IIdentifiable
    {
        public Guid Id { get; set; }
        public List<T> Events { get; set; }
        public int Version { get; set; }
        public bool IsSent { get; set; } = false;
    }
}
