using DTO;
using DAL;

namespace BLL
{
    public class SqlLiteUserLogin
    {
        public readonly IUserLoginRepository UserLoginRepository;
        public SqlLiteUserLogin()
        {
            var str = SystemsManager.Instance.AppSettings.FilePathSetting.SqlLiteConnectString;
            ISqlConnection connectionstr = new SqlLiteConnection(str);
            this.UserLoginRepository = new DAL.SqlLiteUserLogin(connectionstr);
        }
    }
}
