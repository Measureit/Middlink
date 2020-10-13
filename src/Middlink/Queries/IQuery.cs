using Middlink.Messages;

namespace Middlink.Queries
{
    public interface IQuery : IMessage
    {
    }

    public interface IQuery<T> : IQuery
    {
    }
}
