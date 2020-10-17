using System;
using System.Text.Json.Serialization;

namespace Middlink.Messages.Operations
{
    public class OperationPending : IMessage
    {
        public Guid OperationId { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }

        public Guid ResourceId { get; }

        [JsonConstructor]
        public OperationPending(Guid operationId,
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
