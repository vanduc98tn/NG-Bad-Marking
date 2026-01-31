using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public interface IRepositoryFileMachine<TEntity>
    {
        bool LoadFileMachine(string filePath);
        Dictionary<Int32, TEntity> GetAll();
        TEntity GetDevice(int code);
        Dictionary<Int32, TEntity> GetByBlock(string block);
    }
}
