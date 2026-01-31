using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public abstract class BaseRepositorySQL<TEntity> : IRepositorySQL<TEntity>
    {
        public abstract Task<bool> Delete(int id);
        public abstract Task<IEnumerable<TEntity>> GetAll();

        public abstract Task<TEntity> GetById(int id);

        public abstract Task<bool> Insert(TEntity entity);

        public abstract Task<bool> Update(TEntity entity);

    }
}
