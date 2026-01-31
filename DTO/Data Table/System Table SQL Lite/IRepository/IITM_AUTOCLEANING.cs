using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface IITM_AUTOCLEANING:IRepositorySQL<ITM_AUTOCLEANING>
    {
        Task<bool> Exists(string jigID);
        Task<bool> UpdateInTime(string jigID, string inTime);
        Task<bool> UpdateOutTime(string jigID, string outTime);
    }
}
