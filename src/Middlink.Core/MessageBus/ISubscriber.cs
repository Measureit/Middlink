using Middlink.Core.CQRS.Commands;
using Middlink.Core.CQRS.Events;
using Middlink.Core.Exceptions;
using System;

namespace Middlink.Core.MessageBus
{
    public interface ISubscriber
    {
        ISubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
                    Func<TCommand, MiddlinkException, IRejectedEvent> onError = null)
                    where TCommand : ICommand;
        ISubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
                    Func<TEvent, MiddlinkException, IRejectedEvent> onError = null)
                    where TEvent : IDomainEvent;
    }
}
