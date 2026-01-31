
using DTO;
using DAL;


namespace BLL
{
    public class SqlLiteAlarmLog
    {
        public readonly IAlarmLogRepository AlarmLogRepository;
        public SqlLiteAlarmLog()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlLiteConnectString; 
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.AlarmLogRepository = new DAL.SqlLiteAlarmLog(connectionstr);
        }
    }
}
