using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public abstract class BaseRepositoryFileMachine<TEntity> : IRepositoryFileMachine<TEntity>
    {
        public abstract Dictionary<Int32, TEntity> GetAll();

        public abstract Dictionary<int, TEntity> GetByBlock(string block);

        public abstract TEntity GetDevice(int code);

        public abstract bool LoadFileMachine(string filePath);
    }
}
