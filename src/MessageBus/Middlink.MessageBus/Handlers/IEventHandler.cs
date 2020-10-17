using Middlink.Messages;
using Middlink.Messages.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Middlink.MessageBus.Handlers
{
    public interface IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}
