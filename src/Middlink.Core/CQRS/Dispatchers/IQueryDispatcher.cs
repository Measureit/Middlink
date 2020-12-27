using Middlink.Core.CQRS.Queries;
using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
