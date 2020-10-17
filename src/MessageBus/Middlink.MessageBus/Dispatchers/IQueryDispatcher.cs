using Middlink.Messages.Queries;
using System.Threading.Tasks;

namespace Middlink.MessageBus.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
