using Middlink.Exceptions;
using Middlink.Messages.Collections;
using Middlink.Messages.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Middlink.EventSourcing.MongoDb
{
    public class MongoEventStore : IEventStore
    {
        private readonly List<IUncommittedEvent> _uncommittedEvents = new List<IUncommittedEvent>();

        public JsonSerializerOptions JsonSettings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        public IReadOnlyList<IUncommittedEvent> Uncommitted => _uncommittedEvents.AsReadOnly();

        protected IMongoCollection<EventDescriptor<EventDocument>> Collection { get; }

        public MongoEventStore(IMongoDatabase database)
        {
            Collection = database.GetCollection<EventDescriptor<EventDocument>>($"{nameof(EventDescriptor<EventDocument>)}s");
        }

        public async Task<IEnumerable<ICommittedEvent>> GetAllEventsAsync(Guid id)
        {
            var events = await Collection
                .Find(x => x.Id == id)
                .Project(x => x.Events)
                .SingleOrDefaultAsync();

            return events.Select(x => Deserialize(id, x)).OrderBy(x => x.Version).ToList();
        }


        public async Task<IEnumerable<ICommittedEvent>> GetEventsForwardAsync(Guid aggregateId, int version)
        {
            var events = await Collection
               .Find(x => x.Id == aggregateId)
               .Project(x => x.Events.Where(c => c.Version > version))
               .SingleOrDefaultAsync();
            return events.Select(x => Deserialize(aggregateId, x)).OrderBy(x => x.Version).ToList();
        }

        public async Task AppendAsync(AggregateRoot aggregate, IEnumerable<IUncommittedEvent> uncommittedEvents)
        {
            if (await Collection.Find(x => x.Id == aggregate.Id).CountDocumentsAsync().ConfigureAwait(false) == 1)
            {
                var update = Builders<EventDescriptor<EventDocument>>.Update
                .Set(s => s.Version, aggregate.Sequence)
                .Set(s => s.IsSent, false)
                .AddToSetEach(s => s.Events, uncommittedEvents.Select(Serialize));

                var updateResult = await Collection.UpdateOneAsync(x =>
                  x.Id == aggregate.Id &&
                  x.Version == aggregate.Version &&
                  x.IsSent, update).ConfigureAwait(false);

                if (updateResult.ModifiedCount != 1)
                {
                    throw new ConcurrencyException(aggregate.Id, aggregate.Version);
                }
            }
            else
            {
                var eventsDescriptor = new EventDescriptor<EventDocument>
                {
                    Id = aggregate.Id,
                    Version = aggregate.Sequence,
                    Events = uncommittedEvents.Select(Serialize).ToList()
                };
                await Collection.InsertOneAsync(eventsDescriptor);
            }
        }

        public async Task Commit(AggregateRoot aggregate)
        {
            var update = Builders<EventDescriptor<EventDocument>>.Update.Set(s => s.IsSent, true);

            var updateResult = await Collection.UpdateOneAsync(x => x.Id == aggregate.Id && x.Version == aggregate.Sequence, update);

            if (updateResult.ModifiedCount != 1)
            {
                throw new InvalidOperationException();
            }
        }

        public EventDocument Serialize(IUncommittedEvent eventToSerialize)
        {
            var id = eventToSerialize.Metadata.GetValue(MetadataKeys.EventId, value => Guid.Parse(value.ToString()));
            var eventType = eventToSerialize.Metadata.GetValue(MetadataKeys.EventName, value => value.ToString());
            var timestamp = eventToSerialize.Metadata.GetValue(MetadataKeys.Timestamp, value => (DateTime)value);

            var dataJson = JsonSerializer.Serialize(eventToSerialize.Data, JsonSettings);
            var metadataJson = JsonSerializer.Serialize(eventToSerialize.Metadata, JsonSettings);

            var @event = new EventDocument
            {
                Id = id,
                Timestamp = timestamp,
                EventType = eventType,
                Version = eventToSerialize.Version,
                EventData = BsonDocument.Parse(dataJson),
                Metadata = BsonDocument.Parse(metadataJson)
            };

            return @event;
        }

        public ICommittedEvent Deserialize(Guid aggregateId, EventDocument doc)
        {
            var eventType = Type.GetType(doc.Metadata[MetadataKeys.EventClrType].AsString);
            var data = JsonSerializer.Deserialize(doc.EventData.ToJson(), eventType, JsonSettings);
            var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(doc.Metadata.ToJson());
            var version = doc.Metadata[MetadataKeys.EventVersion].AsInt32;

            return new CommittedEvent(aggregateId, version, data as IDomainEvent, new MetadataCollection(metadata));
        }

        public void Dispose()
        {
            _uncommittedEvents.Clear();
        }
    }
}