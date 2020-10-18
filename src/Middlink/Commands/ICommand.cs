using System;

namespace Middlink.Commands
{
    public interface ICommand : IMessage
    {
        Guid AggregateId { get; }
    }
}
