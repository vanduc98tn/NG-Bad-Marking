using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class UserLoginService
    {
        private LoggerDebug logger = new LoggerDebug("UserLoginService");
        private IUserLoginRepository userLoginRepository;
        public UserLoginService(IUserLoginRepository userLoginRepository)
        {
            this.userLoginRepository = userLoginRepository;
        }
        public async Task<bool> ChangePassword(string userName,string newPassword)
        {
            if(this.userLoginRepository == null)
            {
                logger.Create("ChangePassword userLoginRepository = null", LogLevel.Error);
                return false;
            }
            if(string.IsNullOrEmpty(userName))
            {
                logger.Create("ChangePassword input userName = null or userName = Empty", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                logger.Create("ChangePassword input newPassword = null or newPassword = Empty", LogLevel.Error);
                return false;
            }
            return await this.userLoginRepository.ChangePassword(userName, newPassword);
        }
        public async Task<bool> VerifyPassword(string userName, string Password)
        {
            if (this.userLoginRepository == null)
            {
                logger.Create("VerifyPassword userLoginRepository = null", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(userName))
            {
                logger.Create("VerifyPassword input userName = null or userName = Empty", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                logger.Create("VerifyPassword input Password = null or Password = Empty", LogLevel.Error);
                return false;
            }
            return await this.userLoginRepository.CheckPassword(userName, Password);
        }
        public async Task<int> CheckAccess(string userName)
        {
            if (this.userLoginRepository == null)
            {
                logger.Create("CheckAccess userLoginRepository = null", LogLevel.Error);
                return -1;
            }
            if (string.IsNullOrEmpty(userName))
            {
                logger.Create("CheckAccess input userName = null or userName = Empty", LogLevel.Error);
                return -1;
            }
            return await this.userLoginRepository.GetAccess(userName);
        }
    }
}
