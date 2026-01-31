using DTO;
using DAL;

namespace BLL
{
    public class SqlServerITM_DEFLUX
    {
        public readonly IITM_DEFLUX IITM_DEFLUX;
        public SqlServerITM_DEFLUX()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlServerConnectString;
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.IITM_DEFLUX = new DAL.SqlServerITM_DEFLUX(connectionstr);
        }
    }
}
