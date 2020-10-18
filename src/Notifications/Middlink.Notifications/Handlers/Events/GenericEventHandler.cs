using Middlink.MessageBus.Handlers;
using Middlink.Messages;
using Middlink.Messages.Events;
using Middlink.Notifications.Operations;
using Middlink.Notifications.Services.Operations;
using System.Threading.Tasks;

namespace Middlink.Notifications.Handlers.Events
{
    public class GenericEventHandler<T> : IEventHandler<T> where T : class, IDomainEvent
    {
        private readonly IOperationPublisher _operationPublisher;
        private readonly IOperationsStorage _operationsStorage;

        public GenericEventHandler(
            IOperationPublisher operationPublisher,
            IOperationsStorage operationsStorage)
        {
            _operationPublisher = operationPublisher;
            _operationsStorage = operationsStorage;
        }

        public async Task HandleAsync(T @event, ICorrelationContext context)
        {
            switch (@event)
            {
                case IRejectedEvent rejectedEvent:
                    await _operationsStorage.SetAsync(context.Id, context.UserId,
                        context.Name, OperationState.Rejected, context.Resource, context.ResourceId,
                        rejectedEvent.Code, rejectedEvent.Reason);
                    await _operationPublisher.RejectAsync(context, rejectedEvent.Code, rejectedEvent.Reason);
                    return;
                case IDomainEvent _:
                    await _operationsStorage.SetAsync(context.Id, context.UserId,
                        context.Name, OperationState.Completed, context.Resource, context.ResourceId);
                    await _operationPublisher.CompleteAsync(context);
                    return;
            }
        }
    }
}
