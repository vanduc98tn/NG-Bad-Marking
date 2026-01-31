using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using BLL;

namespace GUI
{
    public class AlarmLogs:LogsManager
    {
        private LoggerDebug logger = new LoggerDebug("AlarmLogs");
        public void CreateAlarmLogs(string createdTime, string alarmCode, string message, string solution, string mode)
        {
            try
            {
                var alarmLog = new AlarmLog();
                LogsManager.Instance.alarmLogService.CreateAlarm(alarmLog);
            }
            catch (Exception ex)
            {
                logger.Create("CreateAlarmLogs: " + ex.Message, LogLevel.Error);
            }
        }
        public async Task<IEnumerable<AlarmLog>> GetLatestAlarmLog(int limit)
        {
            try
            {
                var alarmLog = new AlarmLog();
                return await LogsManager.Instance.alarmLogService.GetLatestAlarmLog(limit);
            }
            catch (Exception ex)
            {
                logger.Create("CreateAlarmLogs: " + ex.Message, LogLevel.Error);
            }
            return null;
        }
    }
}
