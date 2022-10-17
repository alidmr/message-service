using System.Linq.Expressions;
using MessageService.Domain.Entities.Base;
using MessageService.Domain.Repositories.Base;
using MessageService.Infrastructure.Context;
using MongoDB.Driver;

namespace MessageService.Infrastructure.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IMessageServiceContext context, string collectionName)
        {
            // _collection = context.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
            _collection = context.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _collection.AsQueryable().ToListAsync();
            }

            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, TEntity entity)
        {
            await _collection.FindOneAndReplaceAsync(x => x.Id == id, entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.FindOneAndDeleteAsync(x => x.Id == id);
        }
    }
}