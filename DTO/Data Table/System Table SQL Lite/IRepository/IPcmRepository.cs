using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface IPcmRepository:IRepositorySQL<PcmData>
    {
        Task<int> SpcTotal(DateTime from, DateTime to);
        Task<int> SpcCounter(int pcmResult, DateTime from, DateTime to);
        Task<int> SpcTotalByLot(String lotId, DateTime from, DateTime to);
        Task<int> SpcCounterByLot(String lotId, int pcmResult, DateTime from, DateTime to);
    }
}
