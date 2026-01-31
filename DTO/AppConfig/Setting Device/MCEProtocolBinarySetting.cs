using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MCEProtocolBinarySetting:TCPSetting
    {
        public MCEProtocolBinarySetting()
        {
            this.Ip = "192.168.3.39";
            this.Port = 1025;
        }
    }
}
