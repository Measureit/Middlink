using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Middlink.Dispatchers;
using Middlink.Exceptions;
using Middlink.Messages;
using Middlink.Messages.Commands;
using Middlink.Messages.Events;
using Middlink.Services;
using Polly;
using System;
using System.Threading.Tasks;

namespace Middlink.MVC.Services.MessageBrokers
{
    public class BusSubscriber : IBusSubscriber
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IApplicationBuilder _app;
        private readonly IBusClient _busClient;
        private readonly int _retries;
        private readonly int _retryInterval;
        public BusSubscriber(IApplicationBuilder app)
        {
            _app = app;
            _logger = _app.ApplicationServices.GetService<ILogger<BusSubscriber>>();
            _serviceProvider = _app.ApplicationServices.GetService<IServiceProvider>();
            _busClient = _serviceProvider.GetService<IBusClient>();
            _retries = 0;
            _retryInterval = 2;
        }

        public IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, InfrastructureException, IRejectedEvent> onError = null)
            where TCommand : ICommand
        {
            _busClient.SubscribeAsync<TCommand>((command, correlationContext) =>
            {
                return TryHandleAsync(command, correlationContext,
                () =>
                {
                    ICommandDispatcher dispatcher = _app.ApplicationServices.CreateScope().ServiceProvider.GetService<ICommandDispatcher>();
                    return dispatcher.DispatchAsync(command, correlationContext);
                }, onError);
            });

            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, InfrastructureException, IRejectedEvent> onError = null)
            where TEvent : IDomainEvent
        {

            _busClient.SubscribeAsync<TEvent>((@event, correlationContext) =>
            {
                return TryHandleAsync(@event, correlationContext,
                () =>
                {
                    IEventDispatcher dispatcher = _app.ApplicationServices.CreateScope().ServiceProvider.GetService<IEventDispatcher>();
                    return dispatcher.DispatchAsync(@event, correlationContext);
                }, onError);
            });

            return this;
        }

        //// Internal retry for services that subscribe to the multiple events of the same types.
        //// It does not interfere with the routing keys and wildcards (see TryHandleWithRequeuingAsync() below).
        private Task TryHandleAsync<TMessage>(TMessage message,
            ICorrelationContext messageContext,
            Func<Task> handle, Func<TMessage, InfrastructureException, IRejectedEvent> onError = null)
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
                    if (exception is InfrastructureException demandException && onError != null)
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
