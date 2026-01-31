using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ModbusCOMSetting: COMSetting
    {
        public ushort AddressSlave { get; set; }
        public ModbusCOMSetting()
        {
            this.AddressSlave = 1;
        }
    }
}
