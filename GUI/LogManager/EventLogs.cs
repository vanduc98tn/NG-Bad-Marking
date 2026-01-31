using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using BLL;

namespace GUI
{
    public class EventLogs: LogsManager
    {
        private LoggerDebug logger = new LoggerDebug("EventLogs");
        public void CreateEventLog(string message, string type="")
        {
            try
            {
                var log = new EventLog(message, type);
                LogsManager.Instance.eventLogService.CreateEvent(log);
            }
            catch (Exception ex)
            {
                logger.Create("CreateEventLog: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
