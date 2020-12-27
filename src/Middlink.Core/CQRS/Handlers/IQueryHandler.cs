using System.Threading.Tasks;

namespace Middlink.Core.CQRS.Handlers
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
