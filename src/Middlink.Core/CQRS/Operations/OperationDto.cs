using System;

namespace Middlink.Core.CQRS.Operations
{

    public class OperationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Resource { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }
        public Guid ResourceId { get; set; }
    }
}
