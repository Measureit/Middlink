using System;
using System.Text.Json.Serialization;

namespace Middlink.Core.CQRS.Events
{
    public class RejectedEvent : DomainEvent, IRejectedEvent
    {
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public RejectedEvent(Guid aggregateId, string reason, string code) : base(aggregateId)
        {
            Reason = reason;
            Code = code;
        }

        public static IRejectedEvent For(Guid aggregateId, string name)
            => new RejectedEvent(aggregateId, $"There was an error when executing: " +
                                 $"{name}", $"{name}_error");
    }
}
