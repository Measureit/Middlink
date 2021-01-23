using System;
using System.Threading.Tasks;

namespace Middlink.Core.MessageBus
{
    public interface IBusClient
    {
        Task PublishAsync<TMessage>(TMessage message, ICorrelationContext context) where TMessage : IMessage;
        Task SubscribeAsync<TMessage>(Func<TMessage, ICorrelationContext, Task> subscribeMethod, string @namespace = null) where TMessage : IMessage;
    }
}
