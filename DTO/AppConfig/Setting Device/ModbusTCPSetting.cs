using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ModbusTCPSetting: TCPSetting
    {
        public ushort Address { get; set; }
        public ModbusTCPSetting()
        {
            this.Address = 1;
        }
    }
}
