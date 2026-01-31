using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface ILotStatusRepository:IRepositorySQL<LotStatus>
    {
        Task<IEnumerable<LotStatus>> GetLotStatusByTime(DateTime from, DateTime to);
        Task<LotStatus> GetMostRecent();
        Task<LotStatus> GetLotStatusByLotId(string lotId);
    }
}
