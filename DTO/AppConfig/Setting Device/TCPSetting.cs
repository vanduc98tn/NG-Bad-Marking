using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TCPSetting
    {

        public string Ip { get; set; }
        public int Port { get; set; }
        public TCPSetting()
        {
            this.Ip = "192.168.3.20";
            this.Port = 502;
        }
    }
}
