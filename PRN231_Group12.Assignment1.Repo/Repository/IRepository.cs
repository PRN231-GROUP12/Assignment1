using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN231_Group12.Assignment1.Repo.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
        TEntity? GetById(object? id);

        IEnumerable<TEntity> Get(
            int? pageIndex = 0,
            int? pageSize = 10,
            params Expression<Func<TEntity, object>>[] includeProperties);
        
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, int? pageIndex, int? pageSize, params Expression<Func<TEntity, object>>[]? includeProperties);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[]? includeProperties);

    }
}