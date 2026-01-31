using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepositoryMES<TEntity>
    {
        Task<TEntity> Send(TEntity entity);
        Task<bool> SendReady(TEntity entity);
    }
}
