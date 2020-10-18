using Microsoft.AspNetCore.SignalR;
using Middlink.Messages;
using Middlink.Notifications.SignalR.Hubs;
using System.Threading.Tasks;
using Middlink.CQRS.Operations.Hubs.Broadcasters;
using Middlink.CQRS.Operations.Operations;
using Middlink.CQRS.Operations.Services.Operations;

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
            => _hubContext.Clients.Group(context.UserId.ToUserGroup()).operationPending(new OperationPending(context.Id,
            context.UserId, context.Name, context.Resource, context.ResourceId));


        public Task CompleteAsync(ICorrelationContext context)
            => _hubContext.Clients.Group(context.UserId.ToUserGroup()).operationCompleted(new OperationCompleted(context.Id,
            context.UserId, context.Name, context.Resource, context.ResourceId));

        public Task RejectAsync(ICorrelationContext context, string code, string message)
            => _hubContext.Clients.Group(context.UserId.ToUserGroup()).operationRejected(new OperationRejected(context.Id,
            context.UserId, context.Name, context.Resource, context.ResourceId, code, message));
    }
}
