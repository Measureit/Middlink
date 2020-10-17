using System;

namespace Middlink.Storage.Entities
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}
