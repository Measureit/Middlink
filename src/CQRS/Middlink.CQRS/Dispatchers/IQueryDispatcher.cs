using Middlink.Queries;
using System.Threading.Tasks;

namespace Middlink.CQRS.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
