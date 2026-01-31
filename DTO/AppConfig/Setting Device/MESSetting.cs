using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MESSetting : TCPSetting
    {
        public string EquimentID { get; set; }
        public MESSetting()
        {
            this.Ip = "192.168.73.105";
            this.Port = 5010;
            this.EquimentID = "AUTO1234";
        }
    }
}
