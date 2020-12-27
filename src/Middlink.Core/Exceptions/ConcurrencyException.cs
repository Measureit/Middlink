using System;

namespace Middlink.Core.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public Guid AggregateId { get; }
        public int ExpectedVersion { get; }

        public ConcurrencyException(Guid aggregateId, int expectedVersion)
        {
            AggregateId = aggregateId;
            ExpectedVersion = expectedVersion;
        }
    }
}
