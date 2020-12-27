using Middlink.Core;
using System;
using System.Text.Json.Serialization;

namespace Middlink.CQRS.Operations.Operations
{
    public class OperationCompleted : IMessage
    {
        public Guid OperationId { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }

        public Guid ResourceId { get; }

        [JsonConstructor]
        public OperationCompleted(Guid operationId,
            Guid userId, string name, string resource, Guid resourceId)
        {
            OperationId = operationId;
            UserId = userId;
            Name = name;
            Resource = resource;
            ResourceId = resourceId;
        }
    }
}
