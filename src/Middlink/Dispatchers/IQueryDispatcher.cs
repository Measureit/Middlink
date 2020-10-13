using System.Threading.Tasks;
using Middlink.Queries;

namespace Middlink.Dispatchers
{
  public interface IQueryDispatcher
  {
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
  }
}
