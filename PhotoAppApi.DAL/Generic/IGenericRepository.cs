using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAppApi.DAL.Generic
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task CreateAsync(TEntity item);
        Task<TEntity> FindByIdAsync(params object?[]? keyValues);
        IQueryable<TEntity> FindWithInclude(Expression<Func<TEntity, bool>> predicate,
                                          params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Read();
        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> predicate);
        Task RemoveAsync(TEntity item);
        Task RemoveAsync(Expression<Func<TEntity, bool>> predicate);
        Task RemoveByIdAsync(params object?[]? keyValues);
        Task UpdateAsync(TEntity item);
    }
}
