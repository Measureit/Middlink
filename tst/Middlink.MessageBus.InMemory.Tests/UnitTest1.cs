using System;
using Xunit;
using Middlink.MessageBus.InMemory;
using System.Threading.Tasks;
using Middlink.Core;
using Middlink.Core.CQRS.Events;

namespace Middlink.MessageBus.InMemory.Tests
{

    public record MemberConfirmed(Guid AggregateId, Guid MemberId, Guid UserId, string Email) : IDomainEvent<Guid>;
    public record MemberConfirmed2(Guid AggregateId, Guid MemberId, Guid UserId, string Email) : IDomainEvent<Guid>;
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var bus = new InMemoryBusClient();
            await bus.SubscribeAsync<MemberConfirmed>((x,y) =>
            {
                var t = x;
                return Task.CompletedTask;
            });

            await bus.SubscribeAsync<MemberConfirmed2>((x, y) =>
            {
                var t = x;
                return Task.CompletedTask;
            }, typeof(MemberConfirmed).FullName);


            await bus.PublishAsync(new MemberConfirmed(Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),"emails"),CorrelationContext.Empty);

        }
    }
}
