
using DTO;
using DAL;

namespace BLL
{
    public class SqlLiteEventLog
    {
        public readonly IEventLogRepository EventLogRepository;
        public SqlLiteEventLog()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlLiteConnectString;
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.EventLogRepository = new DAL.SqlLiteEventLog(connectionstr);
        }
    }
}
