using System;
using Xunit;
using Middlink.MessageBus.InMemory;
using System.Threading.Tasks;
using Middlink.Core;

namespace Middlink.MessageBus.InMemory.Tests
{
    public class M2 : M1
    {
    }
    public class M1 : IMessage
    {
        public string A { get; set; }
        public string B { get; set; }
    }
    public class B1 : IMessage
    {
        public string A { get; set; }
        public string C { get; set; }
    }
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var bus = new InMemoryBusClient();
            await bus.SubscribeAsync<M1>((x,y) =>
            {
                var t = x;
                return Task.CompletedTask;
            });

            await bus.SubscribeAsync<M2>((x, y) =>
            {
                var t = x;
                return Task.CompletedTask;
            }, typeof(M1).FullName);

            await bus.SubscribeAsync<B1>((x, y) =>
            {
                var t = x;
                return Task.CompletedTask;
            }, typeof(M1).FullName);

            await bus.PublishAsync(new M1 { A = "Hi", B = "LOL" },CorrelationContext.Empty);

        }
    }
}
