using Middlink.Core;
using Middlink.Core.MessageBus;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Middlink.MessageBus.InMemory
{
    public class InMemoryBusClient : IBusClient
    {
        private readonly ISubject<(IMessage, ICorrelationContext)> _bus;
        public InMemoryBusClient()
        {
            _bus = new Subject<(IMessage, ICorrelationContext)>();
        }
        public Task PublishAsync<TMessage>(TMessage message, ICorrelationContext context)
            where TMessage : IMessage
        {
            _bus.OnNext((message, context));
            return Task.CompletedTask;
        }
        public Task<IDisposable> SubscribeAsync<TMessage>(Func<TMessage, ICorrelationContext, Task> subscribeMethod, string @namespace = null) where TMessage : IMessage
        {
            var subscription = _bus
              .ObserveOn(Scheduler.Default)
              .Where(message => message.Item1.GetType().FullName.Equals(@namespace ?? typeof(TMessage).FullName))
                   .Select(x => Observable.FromAsync(async () => await Execute<TMessage>(x, subscribeMethod)))
                    .Concat()
                    .Subscribe();
            return Task.FromResult(subscription);
        }

        async Task<Unit> Execute<TMessage>((IMessage, ICorrelationContext) message, Func<TMessage, ICorrelationContext, Task> subscribeMethod)
        {
            try
            {
                if (message.Item1 is TMessage content)
                {
                    await subscribeMethod(content, message.Item2);
                }
                else
                {
                    await subscribeMethod(JsonSerializer.Deserialize<TMessage>(JsonSerializer.Serialize(message.Item1, message.Item1.GetType())), message.Item2);
                }
            }
            catch (Exception)
            {
                //Empty cactch to continue processing
            }
            return Unit.Default;
        }
    }
}
