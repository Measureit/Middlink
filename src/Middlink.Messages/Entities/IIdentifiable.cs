using System;

namespace Middlink.Messages.Entities
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
