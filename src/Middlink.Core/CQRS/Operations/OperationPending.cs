using Middlink.Core;
using System;
using System.Text.Json.Serialization;

namespace Middlink.Core.CQRS.Operations
{
    public class OperationPending : IMessage
    {
        public Guid OperationId { get; }
        public string UserId { get; }
        public string Name { get; }
        public string Resource { get; }

        public Guid ResourceId { get; }

        [JsonConstructor]
        public OperationPending(Guid operationId,
            string userId, string name, string resource, Guid resourceId)
        {
            OperationId = operationId;
            UserId = userId;
            Name = name;
            Resource = resource;
            ResourceId = resourceId;
        }
    }
}
