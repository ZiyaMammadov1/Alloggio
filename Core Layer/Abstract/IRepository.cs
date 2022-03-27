using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core_Layer.Abstract
{
    public interface IRepository<TEntity>
    {
        TEntity Get(int id);
        List<TEntity> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        int Commit();

    }
}
