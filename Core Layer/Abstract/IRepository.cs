using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.Abstract
{
    public interface IRepository<TEntity>
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params string[] includes);
        TEntity Get(int id);
        List<TEntity> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        int Commit();

    }
}
