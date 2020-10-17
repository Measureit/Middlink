using Middlink.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Middlink.MessageBus.Services
{
    public interface IBusClient
    {
        Task PublishAsync<TMessage>(TMessage message, ICorrelationContext context) where TMessage : IMessage;
        Task SubscribeAsync<TMessage>(Func<TMessage, ICorrelationContext, Task> subscribeMethod) where TMessage : IMessage;
    }
}
