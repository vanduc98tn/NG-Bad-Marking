using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public static class LimitLengthDevice
    {
        public static int CheckLengthLimit(DeviceLength modbusDevice, byte funtionCode)
        {
            LimitLengthModbus PLCLengthLimit;
            if (DeviceLength.PLC_LS_Modbus == modbusDevice)
            {
                PLCLengthLimit = new LimitLengthModbusPLCLS();
            }
            else if (DeviceLength.PLC_MITSIBISHI_Modbus == modbusDevice)
            {
                PLCLengthLimit = new LimitLengthModbusPLCMitsubishi();
            }
            else
            {
                PLCLengthLimit = new LimitLengthModbus();
            }
            if (funtionCode == FuntionModbus.ReadCoils) return PLCLengthLimit.ReadCoilStatusLimit();
            else if (funtionCode == FuntionModbus.ReadDiscreteInputs) return PLCLengthLimit.ReadInputStatusLimit();
            else if (funtionCode == FuntionModbus.ReadHoldingRegisters) return PLCLengthLimit.ReadHoldingRegistersLimit();
            else if (funtionCode == FuntionModbus.ReadInputRegisters) return PLCLengthLimit.ReadInputStatusLimit();
            else if (funtionCode == FuntionModbus.WriteSingleCoil) return PLCLengthLimit.ForceSingleCoilLimit();
            else if (funtionCode == FuntionModbus.WriteSingleRegister) return PLCLengthLimit.PresetSingleRegisterLimit();
            else if (funtionCode == FuntionModbus.WriteMultipleCoils) return PLCLengthLimit.ForceMultipleCoilsLimit();
            else if (funtionCode == FuntionModbus.WriteMultipleRegisters) return PLCLengthLimit.PresetMultipleRegistersLimit();
            return 0;
        }
    }
}
