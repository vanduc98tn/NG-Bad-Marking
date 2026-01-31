using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface IEventLogRepository:IRepositorySQL<EventLog>
    {
        Task<int> GetCount();
        Task<IEnumerable<EventLog>> GetPage(int pageIndex, int pageSize);
    }
}
