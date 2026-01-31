using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public static class FuntionModbus
    {
        public static readonly byte ReadCoils = 0x01;
        public static readonly byte ReadDiscreteInputs = 0x02;
        public static readonly byte ReadHoldingRegisters = 0x03;
        public static readonly byte ReadInputRegisters = 0x04;
        public static readonly byte WriteSingleCoil = 0x05;
        public static readonly byte WriteSingleRegister = 0x06;
        public static readonly byte WriteMultipleCoils = 0x0F;
        public static readonly byte WriteMultipleRegisters = 0x10;
    }
}
