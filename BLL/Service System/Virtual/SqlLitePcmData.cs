using DAL;
using DTO;

namespace BLL
{
    public class SqlLitePcmData
    {
        public readonly IPcmRepository PcmRepository;
        public SqlLitePcmData()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlLiteConnectString;
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.PcmRepository = new DAL.SqlLitePcmData(connectionstr);
        }
    }
}
