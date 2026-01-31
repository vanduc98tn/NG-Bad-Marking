using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MESCheck
    {
        public string EquipmentId { get; set; }
        public string Status { get; set; }
        public string LotNo { get; set; }
        public string PCB_Code { get; set; }
        public List<int> MapNG { get; set; }
        public int TotalCount { get; set; }
        public string MES_Result { get; set; }
        public string CheckSum { get; set; }
    }
}
