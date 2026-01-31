using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class EventLogService
    {
        private readonly IEventLogRepository eventLogRepository;
        private LoggerDebug logger = new LoggerDebug("EventLogService");
        
        public EventLogService(IEventLogRepository eventLogRepository)
        {
            this.eventLogRepository = eventLogRepository;
        }
        public void CreateEvent(EventLog eventLog)
        {
            if (this.eventLogRepository==null)
            {
                logger.Create("CreateEvent eventLogRepository = null", LogLevel.Error);
                return;
            }    
            if(eventLog==null)
            {
                logger.Create("CreateEvent input eventLog = null", LogLevel.Error);
                return;
            }
            Task.Run(async () =>
            {
                var result = await this.eventLogRepository.Insert(eventLog);
                if (!result) if (!result) logger.Create("CreateEventLog Faild CMD: " + eventLog.Message, LogLevel.Warning);
            });
        }
        public async Task<IEnumerable<EventLog>> GetPageEvents(int pageIndex, int pageSize)
        {
            if (this.eventLogRepository == null)
            {
                logger.Create("GetPageEvents eventLogRepository = null", LogLevel.Error);
                return null;
            }
            if (pageIndex<0)
            {
                logger.Create("GetPageEvents input pageIndex < 0", LogLevel.Error);
                return null;
            }   
            if(pageSize<=0)
            {
                logger.Create("GetPageEvents input pageSize <= 0", LogLevel.Error);
                return null;
            }
            return await this.eventLogRepository.GetPage(pageIndex, pageSize);
        }
        public async Task<int> GetCount()
        {
            if (this.eventLogRepository == null)
            {
                logger.Create("GetCount eventLogRepository = null", LogLevel.Error);
                return -1;
            }
            return await this.eventLogRepository.GetCount();
        }
    }
}
