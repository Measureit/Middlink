using Microsoft.AspNetCore.SignalR;
using Middlink.Core;
using Middlink.Core.CQRS.Operations;
using Middlink.CQRS.Operations.Operations;
using Middlink.CQRS.Operations.SignalR.Extensions;
using Middlink.CQRS.Operations.SignalR.Hubs;
using System.Threading.Tasks;

namespace Middlink.CQRS.Operations.SignalR.Services.Operations
{
    public class OperationPublisher : IOperationPublisher
    {
        private readonly IHubContext<OperationsHub, IOperationBroadcaster> _hubContext;

        public OperationPublisher(IHubContext<OperationsHub, IOperationBroadcaster> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task PendingAsync(ICorrelationContext context)
        {
            var operation = new OperationPending(context.Id, context.UserId, context.Name, context.Resource, context.ResourceId);
            var destinationGroup = context.SessionId.HasValue ? context.SessionId.Value.ToAnonymousSessionGroup() : context.UserId.ToUserGroup();
            return _hubContext.Clients.Group(destinationGroup).operationPending(operation);
        }

        public Task CompleteAsync(ICorrelationContext context)
        {
            var operation = new OperationCompleted(context.Id,context.UserId, context.Name, context.Resource, context.ResourceId);
            var destinationGroup = context.SessionId.HasValue ? context.SessionId.Value.ToAnonymousSessionGroup() : context.UserId.ToUserGroup();
            return _hubContext.Clients.Group(destinationGroup).operationCompleted(operation);
        }


        public Task RejectAsync(ICorrelationContext context, string code, string message)
        {
            var operation = new OperationRejected(context.Id, context.UserId, context.Name, context.Resource, context.ResourceId, code, message);
            var destinationGroup = context.SessionId.HasValue ? context.SessionId.Value.ToAnonymousSessionGroup() : context.UserId.ToUserGroup();
            return _hubContext.Clients.Group(destinationGroup).operationRejected(operation);
        }
    }
}
