using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

        public Task SubscribeAsync<TMessage>(Func<TMessage, ICorrelationContext, Task> subscribeMethod) where TMessage : IMessage
        {
            var subscription = _bus
              .Where(message => message.Item1.GetType().Equals(typeof(TMessage)))
              .Subscribe(async message =>
              {
                  try
                  {
                      await subscribeMethod((TMessage)message.Item1, message.Item2);
                  }
                  catch (Exception)
                  {
                      //Empty cactch to continue processing
                  }
              });
            return Task.CompletedTask;
        }
    }
}
