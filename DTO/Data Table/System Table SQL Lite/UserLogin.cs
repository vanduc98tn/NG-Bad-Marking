using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Access { get; set; }

        public UserLogin()
        {

        }
        public UserLogin(string userName,string password,int access)
        {
            this.Id = 0;
            this.UserName = userName;
            this.Password = password;
            this.Access = access;
        }
    }
}
