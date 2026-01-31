using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LimitLengthModbus
    {
        public virtual int ReadCoilStatusLimit()
        {
            return 2000;
        }
        public virtual int ReadInputStatusLimit()
        {
            return 2000;
        }
        public virtual int ReadHoldingRegistersLimit()
        {
            return 125;
        }
        public virtual int ReadInputRegistersLimit()
        {
            return 125;
        }
        public virtual int ForceSingleCoilLimit()
        {
            return 1;
        }
        public virtual int PresetSingleRegisterLimit()
        {
            return 1;
        }
        public virtual int ForceMultipleCoilsLimit()
        {
            return 1968;
        }
        public virtual int PresetMultipleRegistersLimit()
        {
            return 120;
        }
    }
}
