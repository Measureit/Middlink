using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.EventSource
{
    public struct MetadataKeys
    {
        public const string AggregateTypeFullname = "aggregateTypeFullname";
        public const string AggregateId = "aggregateId";
        public const string AggregateSequenceNumber = "aggregateSequenceNumber";

        public const string EventId = "eventId";
        public const string EventClrType = "eventClrType";
        public const string EventName = "eventName";
        public const string EventVersion = "eventVersion";
        public const string EventIgnore = "ignore";

        public const string CorrelationId = "correlationId";
        public const string Timestamp = "timestamp";

        public const string SnapshotClrType = "snapshotClrType";
        public const string SnapshotId = "snapshotId";
        public const string SnapshotName = "snapshotName";
    }
}
