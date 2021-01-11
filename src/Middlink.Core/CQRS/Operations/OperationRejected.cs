using Middlink.Core;
using System;
using System.Text.Json.Serialization;

namespace Middlink.Core.CQRS.Operations
{
    public class OperationRejected : IMessage
    {
        public Guid OperationId { get; }
        public string UserId { get; }
        public string Name { get; }
        public string Resource { get; }
        public string Code { get; }
        public string Message { get; }
        public Guid ResourceId { get; }

        [JsonConstructor]
        public OperationRejected(Guid operationId,
                  string userId, string name, string resource, Guid resourceId,
                    string code, string message)
        {
            OperationId = operationId;
            UserId = userId;
            Name = name;
            Resource = resource;
            ResourceId = resourceId;
            Code = code;
            Message = message;
        }
    }
}
