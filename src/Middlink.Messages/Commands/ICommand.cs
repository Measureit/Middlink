using System;
using System.Collections.Generic;
using System.Text;

namespace Middlink.Messages.Commands
{
    public interface ICommand : IMessage
    {
        Guid AggregateId { get; }
    }
}
