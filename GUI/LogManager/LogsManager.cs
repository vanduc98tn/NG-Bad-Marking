using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using BLL;

namespace GUI
{
    public class LogsManager
    {
        private static LogsManager instance = new LogsManager();
        public static LogsManager Instance = instance;
        //Debug Log.
        private LoggerDebug logger = new LoggerDebug("LogsManager");
        //Event Log.
        public EventLogService eventLogService;
        public EventLogs EventLogs;
        //Alarm Log.
        public AlarmLogService alarmLogService;
        public AlarmLogs AlarmLogs;
        //LotStatus Log.
        public LotStatusService lotStatusService;
        public LotStatusLogs LotStatusLogs;
        //PCMData Log.
        public PcmService pcmService;
        public PCMDataLogs PCMDataLogs;

        public void Startup()
        {
            this.LoadEventLog();
            this.LoadAlarmLog();
            this.LoadStatusLog();
            this.LoadPCMLog();
        }
        private void LoadEventLog()
        {
            SqlLiteEventLog eventLog = new SqlLiteEventLog();
            this.eventLogService = new EventLogService(eventLog.EventLogRepository);
            this.EventLogs = new EventLogs();
        }
        private void LoadAlarmLog()
        {
            SqlLiteAlarmLog alarmLog = new SqlLiteAlarmLog();
            this.alarmLogService = new AlarmLogService(alarmLog.AlarmLogRepository);
            this.AlarmLogs = new AlarmLogs();
        }
        private void LoadStatusLog()
        {
            SqlLiteLotStatus statusLog = new SqlLiteLotStatus();
            this.lotStatusService = new LotStatusService(statusLog.LotStatusRepository);
            this.LotStatusLogs = new LotStatusLogs();
        }
        private void LoadPCMLog()
        {
            SqlLitePcmData pcmLog = new SqlLitePcmData();
            this.pcmService = new PcmService(pcmLog.PcmRepository);
            this.PCMDataLogs = new PCMDataLogs();
        }
    }
}
