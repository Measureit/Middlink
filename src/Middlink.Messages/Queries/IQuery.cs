using Middlink.Messages;

namespace Middlink.Messages.Queries
{
    public interface IQuery : IMessage
    {
    }

    public interface IQuery<T> : IQuery
    {
    }
}
