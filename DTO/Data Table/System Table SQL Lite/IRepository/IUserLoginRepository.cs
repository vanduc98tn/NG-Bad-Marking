using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DTO
{
    public interface IUserLoginRepository:IRepositorySQL<UserLogin>
    {
        Task<bool> CheckPassword(string userName,string password);
        Task<int> GetAccess(string userName);
        Task<bool> ChangePassword(string userName, string newPassword);
    }
}
