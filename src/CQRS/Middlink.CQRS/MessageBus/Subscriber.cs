using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Middlink.Commands;
using Middlink.CQRS.Dispatchers;
using Middlink.Events;
using Middlink.Exceptions;
using Middlink.MessageBus;
using Polly;
using System;
using System.Threading.Tasks;

namespace Middlink.CQRS.MessageBus
{
    public class Subscriber : ISubscriber
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceProvider _appServiceProvider;
        private readonly IBusClient _busClient;
        private readonly int _retries;
        private readonly int _retryInterval;
        public Subscriber(IServiceProvider appServiceProvider)
        {
            _appServiceProvider = appServiceProvider;
            _logger = (ILogger<Subscriber>)_appServiceProvider.GetService(typeof(ILogger<Subscriber>));
            _serviceProvider = (IServiceProvider)_appServiceProvider.GetService(typeof(IServiceProvider));
            _busClient = (IBusClient)_serviceProvider.GetService(typeof(IBusClient));
            _retries = 0;
            _retryInterval = 2;
        }

        public ISubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, MiddlinkException, IRejectedEvent> onError = null)
            where TCommand : ICommand
        {
            _busClient.SubscribeAsync<TCommand>((command, correlationContext) =>
            {
                return TryHandleAsync(command, correlationContext,
                () =>
                {
                    ICommandDispatcher dispatcher = _appServiceProvider.CreateScope().ServiceProvider.GetService<ICommandDispatcher>();
                    return dispatcher.DispatchAsync(command, correlationContext);
                }, onError);
            });

            return this;
        }

        public ISubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, MiddlinkException, IRejectedEvent> onError = null)
            where TEvent : IDomainEvent
        {

            _busClient.SubscribeAsync<TEvent>((@event, correlationContext) =>
            {
                return TryHandleAsync(@event, correlationContext,
                () =>
                {
                    IEventDispatcher dispatcher = _appServiceProvider.CreateScope().ServiceProvider.GetService<IEventDispatcher>();
                    return dispatcher.DispatchAsync(@event, correlationContext);
                }, onError);
            });

            return this;
        }

        //// Internal retry for services that subscribe to the multiple events of the same types.
        //// It does not interfere with the routing keys and wildcards (see TryHandleWithRequeuingAsync() below).
        private Task TryHandleAsync<TMessage>(TMessage message,
            ICorrelationContext messageContext,
            Func<Task> handle, Func<TMessage, MiddlinkException, IRejectedEvent> onError = null)
        {
            var currentRetry = 0;
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(_retries, i => TimeSpan.FromSeconds(_retryInterval));

            var messageName = message.GetType().Name;

            return retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var retryMessage = currentRetry == 0
                          ? string.Empty
                          : $"Retry: {currentRetry}'.";
                    _logger.LogInformation($"Handling a message: '{messageName}' " +
                                             $"with correlation id: '{messageContext.Id}'. {retryMessage}");

                    await handle();

                    _logger.LogInformation($"Handled a message: '{messageName}' " +
                                             $"with correlation id: '{messageContext.Id}'. {retryMessage}");

                }
                catch (Exception exception)
                {
                    currentRetry++;
                    _logger.LogError(exception, exception.Message);
                    if (exception is MiddlinkException demandException && onError != null)
                    {
                        var rejectedEvent = onError(message, demandException);
                        await _busClient.PublishAsync(rejectedEvent, messageContext);
                        _logger.LogInformation($"Published a rejected event: '{rejectedEvent.GetType().Name}' " +
                                                 $"for the message: '{messageName}' with correlation id: '{messageContext.Id}'.");

                    }

                    throw new Exception($"Unable to handle a message: '{messageName}' " +
                                          $"with correlation id: '{messageContext.Id}', " +
                                          $"retry {currentRetry - 1}/{_retries}...");
                }
            });
        }
    }
}
