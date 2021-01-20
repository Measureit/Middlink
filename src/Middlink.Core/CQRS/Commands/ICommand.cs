using System;

namespace Middlink.Core.CQRS.Commands
{
    public interface ICommand : IMessage
    {
    }
    public interface ICommand<TKey> : ICommand
    {
        TKey AggregateId { get; }
    }
}
