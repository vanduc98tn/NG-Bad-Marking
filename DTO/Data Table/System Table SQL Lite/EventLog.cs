using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EventLog
    {
        public int Id { get; set; }
        public string CreatedTime { get; set; }
        public string Message { get; set; }
        public string EventType { get; set; }
        public EventLog()
        {

        }
        public EventLog(String messageEventLog,string eventType)
        {
            this.Id = 0;
            this.CreatedTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
            this.Message = messageEventLog;
            this.EventType = eventType;
        }
    }
}
