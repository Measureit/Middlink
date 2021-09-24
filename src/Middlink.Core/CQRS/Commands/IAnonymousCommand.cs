using System;

namespace Middlink.Core.CQRS.Commands
{
    public interface IAnonymousCommand : ICommand
    {
        Guid SessionId { get; }
    }

    public interface IAnonymousCommand<TKey> : ICommand<TKey>, IAnonymousCommand
    {
    }
}
