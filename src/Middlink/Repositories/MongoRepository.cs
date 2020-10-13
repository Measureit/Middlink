using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Middlink.Entities;
using Middlink.Messages.Entities;
using Middlink.Queries;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace Middlink.Repositories
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : IIdentifiable
    {
        protected IMongoCollection<TEntity> Collection { get; }

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<TEntity>(collectionName);
        }

        public async ValueTask<TEntity> GetAsync(Guid id)
            => await GetAsync(e => e.Id == id);

        public async ValueTask<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).SingleOrDefaultAsync();

        public async ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).ToListAsync();

        public async ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderSelector)
            => await Collection.Find(predicate).SortBy(orderSelector).ToListAsync();

        public async ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderSelector, int limit)
            => await Collection.Find(predicate).SortBy(orderSelector).Limit(limit).ToListAsync();

        public async ValueTask<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int limit)
            => await Collection.Find(predicate).Limit(limit).ToListAsync();

        public async Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
                TQuery query) where TQuery : PagedQueryBase
            => await Collection.AsQueryable().Where(predicate).PaginateAsync(query);

        public async Task AddAsync(TEntity entity)
            => await Collection.InsertOneAsync(entity);

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
            => await Collection.InsertManyAsync(entities);

        public async Task UpdateAsync(TEntity entity)
            => await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

        public async Task DeleteAsync(Guid id)
            => await Collection.DeleteOneAsync(e => e.Id == id);

        public async ValueTask<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Find(predicate).AnyAsync();
    }
}
