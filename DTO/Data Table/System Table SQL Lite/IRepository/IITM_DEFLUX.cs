using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface IITM_DEFLUX:IRepositorySQL<ITM_DEFLUX>
    {
        Task<bool> Exists(string jigID);
        Task<bool> UpdateOutTime(string jigID, string outTime);
        Task<bool> DeleteByJIG(string jigID);
    }
}
