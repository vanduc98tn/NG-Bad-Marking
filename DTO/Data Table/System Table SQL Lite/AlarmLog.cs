using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AlarmLog
    {
        public int Id { get; set; }
        public string CreatedTime { get; set; }
        public string AlarmCode { get; set; }
        public string Message { get; set; }
        public string Solution { get; set; }
        public string Mode { get; set; }
        public AlarmLog()
        {

        }
        public AlarmLog(string alarmCode, string message, string solution, string mode)
        {
            this.Id = 0;
            this.CreatedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            this.AlarmCode = alarmCode;
            this.Message = message;
            this.Solution = solution;
            this.Mode = mode;
        }
    }
}
