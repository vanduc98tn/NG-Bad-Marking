
using DAL;
using DTO;

namespace BLL
{
    public class SqlLiteLotStatus
    {
        public readonly ILotStatusRepository LotStatusRepository;
        public SqlLiteLotStatus()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlLiteConnectString;
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.LotStatusRepository = new DAL.SqlLiteLotStatus(connectionstr);
        }
    }
}
