using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PcmData
    {
        public int Id { get; set; }
        public string LotId { get; set; }
        public string Qr { get; set; }
        public string Result { get; set; }
        public string UpdateTime { get; set; }
        public PcmData()
        {

        }
        public PcmData(string lotId,string qr,string result)
        {
            this.Id = 0;
            this.LotId = lotId;
            this.Qr = qr;
            this.Result = result;
            this.UpdateTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
        }
    }
}
