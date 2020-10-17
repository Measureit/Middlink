using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


namespace Middlink.EventSourcing.MongoDb
{
    public class EventDocument
    {
        [BsonElement("id")]
        public Guid Id { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; }

        [BsonElement("version")]
        public int Version { get; set; }

        [BsonElement("eventData")]
        public BsonDocument EventData { get; set; }

        [BsonElement("metadata")]
        public BsonDocument Metadata { get; set; }
    }
}
