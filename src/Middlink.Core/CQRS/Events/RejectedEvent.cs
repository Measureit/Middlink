using System;
using System.Text.Json.Serialization;

namespace Middlink.Core.CQRS.Events
{
    public class RejectedEvent<TKey> : DomainEvent<TKey>, IRejectedEvent<TKey>
    {
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public RejectedEvent(TKey aggregateId, string reason, string code) : base(aggregateId)
        {
            Reason = reason;
            Code = code;
        }

        public static IRejectedEvent<TKey> For(TKey aggregateId, string name)
            => new RejectedEvent<TKey>(aggregateId, $"There was an error when executing: " +
                                 $"{name}", $"{name}_error");
    }
}
