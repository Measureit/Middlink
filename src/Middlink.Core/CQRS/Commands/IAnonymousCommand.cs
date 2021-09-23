using System;

namespace Middlink.Core.CQRS.Commands
{
    public interface IAnonymousCommand : ICommand
    {
        Guid SessionId { get; set; }
    }

    public interface IAnonymousCommand<TKey> : ICommand<TKey>, IAnonymousCommand
    {
    }
}
