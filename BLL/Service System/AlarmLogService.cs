using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class AlarmLogService
    {
        private LoggerDebug logger = new LoggerDebug("AlarmLogService");
        private readonly IAlarmLogRepository alarmLogRepository;
        public AlarmLogService(IAlarmLogRepository alarmLogRepository)
        {
            this.alarmLogRepository = alarmLogRepository;
        }
        public void CreateAlarm(AlarmLog alarmLog)
        {
            if(this.alarmLogRepository==null)
            {
                logger.Create("CreateAlarm alarmLogRepository = null", LogLevel.Error);
                return;
            }    
            if(alarmLog==null)
            {
                logger.Create("CreateAlarm input alarmLog = null", LogLevel.Error);
                return;
            }
            Task.Run(async() => {
                var result = await this.alarmLogRepository.Insert(alarmLog);
                if (!result) logger.Create("CreateAlarmLogs Faild CMD: " + alarmLog.Message, LogLevel.Warning);
            });
        }
        public async Task<IEnumerable<AlarmLog>> GetLatestAlarmLog(int limit)
        {
            if (this.alarmLogRepository == null)
            {
                logger.Create("GetLatestAlarmLog alarmLogRepository = null", LogLevel.Error);
                return null;
            }
            if (limit<0)
            {
                logger.Create("GetLatestAlarmLog input limit < 0", LogLevel.Error);
                return null;
            }
            return await this.alarmLogRepository.GetLimitAlarms(limit);
        }
    }
}
