using System;

namespace Middlink.Core.Storage
{
    public interface IIdentifiable : IIdentifiable<Guid>
    {

    }

    public interface IIdentifiable<T>
    {
        Guid Id { get; }
    }
}
