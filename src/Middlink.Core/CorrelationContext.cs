using Middlink.Core.Extensions;
using System;
using System.Text.Json.Serialization;

namespace Middlink.Core
{
    public class CorrelationContext : ICorrelationContext
    {
        public Guid Id { get; }
        public string UserId { get; }
        public Guid ResourceId { get; }
        public string TraceId { get; }
        public string ConnectionId { get; }
        public string Name { get; }
        public string Origin { get; }
        public string Resource { get; }
        public string Culture { get; }
        public int Retries { get; set; }
        public DateTime CreatedAt { get; }
        public Guid? SessionId { get;}

        public CorrelationContext()
        {
        }

        private CorrelationContext(Guid id)
        {
            Id = id;
        }

        [JsonConstructor]
        private CorrelationContext(Guid id, string userId, Guid resourceId, string traceId,
            string connectionId, string executionId, string name, string origin, string culture, string resource, int retries, Guid? sessionId = null)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            TraceId = traceId;
            ConnectionId = connectionId;
            Name = string.IsNullOrWhiteSpace(name) ? string.Empty : GetName(name);
            Origin = string.IsNullOrWhiteSpace(origin) ? string.Empty :
                origin.StartsWith("/") ? origin.Remove(0, 1) : origin;
            Culture = culture;
            Resource = resource;
            Retries = retries;
            SessionId = sessionId;
            CreatedAt = DateTime.UtcNow;
        }

        public static ICorrelationContext Empty
            => new CorrelationContext();

        public static ICorrelationContext FromId(Guid id)
            => new CorrelationContext(id);

        public static ICorrelationContext From<T>(ICorrelationContext context)
            => Create<T>(context.Id, context.UserId, context.ResourceId, context.TraceId, context.ConnectionId,
                context.Origin, context.Culture, context.Resource, context.SessionId);

        public static ICorrelationContext Create<T>(Guid id, string userId, Guid resourceId, string origin,
            string traceId, string connectionId, string culture, string resource = "", Guid? sessionId = null)
            => new CorrelationContext(id, userId, resourceId, traceId, connectionId, string.Empty, typeof(T).Name, origin, culture,
                resource, 0, sessionId);

        private static string GetName(string name) => name.Underscore().ToLowerInvariant();
    }
}
