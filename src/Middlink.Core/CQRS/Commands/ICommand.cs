using System;

namespace Middlink.Core.CQRS.Commands
{
    public interface ICommand : IMessage
    {
        Guid AggregateId { get; }
    }
}
