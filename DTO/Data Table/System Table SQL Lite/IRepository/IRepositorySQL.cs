
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTO
{
    public interface IRepositorySQL<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        Task<bool> Insert(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(int id);
    }
}
