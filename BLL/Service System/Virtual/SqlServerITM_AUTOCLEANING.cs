using DTO;
using DAL;

namespace BLL
{
    public class SqlServerITM_AUTOCLEANING
    {
        public readonly IITM_AUTOCLEANING IITM_AUTOCLEANING;
        public SqlServerITM_AUTOCLEANING()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlServerConnectString;
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.IITM_AUTOCLEANING = new DAL.SqlServerITM_AUTOCLEANING(connectionstr);
        }
    }
}
