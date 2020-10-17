using Middlink.Messages.Queries;
using Middlink.Storage.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Middlink.Storage
{
    public interface IRepository<TEntity> where TEntity : IIdentifiable
    {
        ValueTask<TEntity> GetAsync(Guid id);
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
