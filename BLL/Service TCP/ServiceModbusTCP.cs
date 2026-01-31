using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DTO;
using DAL;

namespace BLL
{
    class ServiceModbusTCP:Device
    {
        LoggerDebug logger = new LoggerDebug("ServiceModbusTCP");
        private SemaphoreSlim modbusSemaphore = new SemaphoreSlim(1, 1);
        private ModbusTCP modbusTCP;
        private DeviceLength modbusDevice;
        public ServiceModbusTCP(DeviceLength modbusDevice, ModbusTCPSetting modbusTCPSetting)
        {
            this.modbusTCP = new ModbusTCP(modbusTCPSetting.Address, modbusTCPSetting.Ip, modbusTCPSetting.Port);
            this.modbusDevice = modbusDevice;
        }
        public void Close()
        {
            this.modbusTCP.Close();
        }
        public async Task<bool> Open()
        {
            try
            {
                return await this.modbusTCP.Open();
            }
            catch(Exception ex)
            {
                logger.Create("Open: " + ex.Message,LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> ReadBit(ushort startAddress, string DeviceName = "")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadBit input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.modbusTCP.ReadBit(startAddress);
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async Task<List<bool>> ReadMultiBits(ushort startAddress, ushort lengh, string DeviceName = "")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                var defaults = new List<bool>();
                if (startAddress > 65535)
                {
                    logger.Create("ReadMultiBits input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    for (int i = 0; i < lengh; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                if (lengh > LimitLengthDevice.CheckLengthLimit(this.modbusDevice, FuntionModbus.ReadCoils))
                {
                    logger.Create("ReadMultiBits input lengh is exceed the allowable limit, lengh should be less than 2000.", LogLevel.Warning);
                    for (int i = 0; i < lengh; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                return await this.modbusTCP.ReadMultiBits(startAddress, lengh);
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async Task<short> ReadSignedWord(ushort startAddress, string DeviceName = "")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadSignedWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return 0;
                }
                return await this.modbusTCP.ReadSignedWord(startAddress);
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async Task<short[]> ReadMultiSignedWords(ushort startAddress, ushort length, string DeviceName = "")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadMultiSignedWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }
                if (length > LimitLengthDevice.CheckLengthLimit(this.modbusDevice, FuntionModbus.ReadHoldingRegisters))
                {
                    logger.Create("ReadMultiSignedWords input lengh is exceed the allowable limit, lengh should be less than 125.", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }
                return await this.modbusTCP.ReadMultiSignedWords(startAddress, length);
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async Task<int> ReadSignedDWord(ushort startAddress, string DeviceName = "")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadSignedDWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return 0;
                }
                return await this.modbusTCP.ReadSignedDWord(startAddress);
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async Task<int[]> ReadMultiSignedDWords(ushort startAddress, ushort length, string DeviceName = "")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadMultiSignedDWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return Enumerable.Repeat((int)0, length).ToArray();
                }
                if (length > (LimitLengthDevice.CheckLengthLimit(this.modbusDevice, FuntionModbus.ReadHoldingRegisters) / 2))
                {
                    logger.Create("ReadMultiSignedDWords input lengh is exceed the allowable limit, lengh should be less than 62.", LogLevel.Warning);
                    return Enumerable.Repeat((int)0, length).ToArray();
                }
                return await this.modbusTCP.ReadMultiSignedDWords(startAddress, length);
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async Task<bool> WriteBit(ushort startAddress, bool value, string DeviceName = "")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("WriteBit input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.modbusTCP.WriteBit(startAddress, value);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }

        }
        public async Task<bool> WriteSignedWord(ushort startAddress, short value, string DeviceName = "")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("WriteSignedWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                SystemsManager.Instance.isWriteDevice = false;
                return await this.modbusTCP.WriteSignedWord(startAddress, value);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
            
        }
        public async Task<bool> WriteSignedDWord(ushort startAddress, int value, string DeviceName = "")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("WriteSignedDWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.modbusTCP.WriteMultiDWords(startAddress, new int[] { value });
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }

        }
        public async Task<bool> WriteMultiBits(ushort startAddress, bool[] values, string DeviceName = "")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("WriteMultiBits input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                if (values.Length > LimitLengthDevice.CheckLengthLimit(this.modbusDevice, FuntionModbus.WriteMultipleCoils))
                {
                    logger.Create("WriteMultiBits input values is exceed the allowable limit, values should be less than 1968.", LogLevel.Warning);
                    return false;
                }
                return await this.modbusTCP.WriteMultiBits(startAddress, values);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }

        }
        public async Task<bool> WriteMultiWords(ushort startAddress, short[] values, string DeviceName = "")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("WriteMultiWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                if (values.Length > LimitLengthDevice.CheckLengthLimit(this.modbusDevice, FuntionModbus.WriteMultipleRegisters))
                {
                    logger.Create("WriteMultiWords input values is exceed the allowable limit, values should be less than 120.", LogLevel.Warning);
                    return false;
                }
                return await this.modbusTCP.WriteMultiWords(startAddress, values);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }

        }
        public async Task<bool> WriteMultiDWords(ushort startAddress, int[] values, string DeviceName = "")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("WriteMultiDWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                if (values.Length > (LimitLengthDevice.CheckLengthLimit(this.modbusDevice, FuntionModbus.WriteMultipleRegisters) / 2))
                {
                    logger.Create("WriteMultiDWords input values is exceed the allowable limit, values should be less than 60.", LogLevel.Warning);
                    return false;
                }
                return await this.modbusTCP.WriteMultiDWords(startAddress, values);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }
    }
}
