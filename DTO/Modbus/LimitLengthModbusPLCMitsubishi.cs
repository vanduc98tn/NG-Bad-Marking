using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LimitLengthModbusPLCMitsubishi: LimitLengthModbus
    {
        public override int PresetMultipleRegistersLimit()
        {
            return 123;
        }
    }
}
