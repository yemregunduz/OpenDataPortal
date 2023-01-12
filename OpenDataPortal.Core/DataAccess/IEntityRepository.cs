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
    public interface IEntityRepository<TEntity>
        where TEntity : BaseEntity,new()
    {

        #region Get Methods 
        Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                   int index = 0, int size = 10, bool enableTracking = true, CancellationToken cancellationToken = default);
        Task<IPaginate<TEntity>> GetListByDynamicAsync(Dynamic dynamic,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                         int index = 0, int size = 10, bool enableTracking = true,
                                         CancellationToken cancellationToken = default);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>,
                IIncludableQueryable<TEntity, object>> include = null, bool enableTracking = true);
        Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool tracking = true);
        #endregion
        #region Insert Methods
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Add(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        #endregion
        #region Delete Methods
        Task<int> DeleteAsync(TEntity entity);
        int Delete(TEntity entity);
        Task<int> DeleteByIdAsync(Guid id);
        int DeleteById(Guid id);
        bool DeleteRange(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion
        #region Update Methods
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        #endregion
        #region SaveChanges Methods
        Task<int> SaveChangesAsync();
        int SaveChanges();
        #endregion
    }
}
