using Middlink.Exceptions;
using Middlink.Messages.Commands;
using Middlink.Messages.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Services
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
                    Func<TCommand, MiddlinkException, IRejectedEvent> onError = null)
                    where TCommand : ICommand;
        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
                    Func<TEvent, MiddlinkException, IRejectedEvent> onError = null)
                    where TEvent : IDomainEvent;
  }
}
