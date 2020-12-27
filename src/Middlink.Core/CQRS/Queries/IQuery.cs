namespace Middlink.Core.CQRS.Queries
{
    public interface IQuery : IMessage
    {
    }

    public interface IQuery<T> : IQuery
    {
    }
}
