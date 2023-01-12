using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OpenDataPortal.Core.DynamicQuery;
using OpenDataPortal.Core.Entities;
using OpenDataPortal.Core.Wrappers.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenDataPortal.Core.DataAccess
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : BaseEntity, new()
        where TContext : DbContext
    {
        protected TContext Context;
        public DbSet<TEntity> Table => Context.Set<TEntity>();
        public EfEntityRepositoryBase(TContext context)
        {
            Context = context ?? throw new ArgumentException(nameof(context));
        }
        #region Insert Methods
        public virtual TEntity Add(TEntity entity)
        {
            Table.Add(entity);
            SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null && !entities.Any())
                return null;
            Table.AddRange(entities);
            SaveChanges();
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null && !entities.Any())
                return null;
            await Table.AddRangeAsync(entities);
            await SaveChangesAsync();
            return entities;
        }
        #endregion
        #region Delete Methods
        public virtual int Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Table.Attach(entity);
            }
            Table.Remove(entity);
            return SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Table.Attach(entity);
            }
            Table.Remove(entity);
            return await SaveChangesAsync();
        }

        public virtual int DeleteById(Guid id)
        {
            var entity = Table.Find(id);
            return Delete(entity);
        }

        public virtual async Task<int> DeleteByIdAsync(Guid id)
        {
            var entity = await Table.FindAsync(id);
            return await DeleteAsync(entity);
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            Context.RemoveRange(predicate);
            return SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Context.RemoveRange(predicate);
            return await SaveChangesAsync() > 0;
        }
        #endregion
        #region Get Methods
        public virtual async Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(data => data.Id == id);
        }

        public virtual async Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int index = 0, int size = 10, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsQueryable();
            if (!enableTracking)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToPaginateAsync(index, size, 0, cancellationToken);

            return await query.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public virtual async Task<IPaginate<TEntity>> GetListByDynamicAsync(Dynamic dynamic, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Table.AsQueryable().ToDynamic(dynamic);
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include != null)
                queryable = include(queryable);
            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool enableTracking = true)
        {
            var query = Table.AsQueryable();
            if (!enableTracking)
                query = Table.AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }
        #endregion
        #region Update Methods
        public virtual TEntity Update(TEntity entity)
        {
            Table.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Table.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
            return entity;
        }
        #endregion
        #region SaveChanges Methods
        public virtual int SaveChanges()
            => Context.SaveChanges();

        public virtual async Task<int> SaveChangesAsync()
            => await Context.SaveChangesAsync();
        #endregion

    }
}
