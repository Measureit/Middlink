using Middlink.Commands;
using Middlink.Events;
using Middlink.Exceptions;
using System;

namespace Middlink.CQRS.MessageBus
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
