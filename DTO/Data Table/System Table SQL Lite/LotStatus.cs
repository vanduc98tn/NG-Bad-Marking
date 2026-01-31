using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LotStatus
    {
        public int Id { get; set; }
        public string LotId { get; set; }
        public string TimeUpdate { get; set; }
        public int InputCount { get; set; }
        public int TotalCount { get; set; }
        public int OKCount { get; set; }
        public int NGCount { get; set; }
        public int EMCount { get; set; }
        public string TotalTime { get; set; }
        public LotStatus()
        {

        }
        public LotStatus(string lotId, int inputCount, int totalCount, int okCount,int ngCount,int emCount)
        {
            this.Id = 0;
            this.LotId = lotId;
            this.TimeUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            this.InputCount = inputCount;
            this.TotalCount = totalCount;
            this.OKCount = okCount;
            this.NGCount = ngCount;
            this.EMCount = emCount;
            this.TotalTime = "";
        }
    }
}
