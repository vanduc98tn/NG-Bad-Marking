using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface IAlarmLogRepository:IRepositorySQL<AlarmLog>
    {
        Task<IEnumerable<AlarmLog>> GetLimitAlarms(int limit);
    }
}
