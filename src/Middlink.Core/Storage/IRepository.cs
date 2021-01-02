using Middlink.Core.CQRS.Queries;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Middlink.Core.Storage
{
    public interface IRepository<TEntity> : IRepository<TEntity, Guid> 
        where TEntity : IIdentifiable<Guid> { }
    public interface IRepository<TEntity, TKey> 
        where TEntity : IIdentifiable<TKey>
    {
        ValueTask<TEntity> GetAsync(TKey id);
        ValueTask<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderSelector);
        ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderSelector, int limit);
        ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int limit);
        Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate, TQuery query) where TQuery : PagedQueryBase;
        Task AddAsync(TEntity entity);
        Task AddManyAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        ValueTask<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
