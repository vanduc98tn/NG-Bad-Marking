using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class ServiceMCProtocolBinaryTCP : Device
    {
        LoggerDebug logger = new LoggerDebug("ServiceMCProtocolBinaryTCP");
        private SemaphoreSlim modbusSemaphore = new SemaphoreSlim(1, 1);
        private MCProtocolBinary mcProtocolBinary;
        public ServiceMCProtocolBinaryTCP(DeviceLength modbusDevice, MCEProtocolBinarySetting modbusTCPSetting)
        {
            this.mcProtocolBinary = new MCProtocolBinary(modbusTCPSetting.Ip, modbusTCPSetting.Port);
        }


        public void Close()
        {
            this.mcProtocolBinary.Close();
        }

        public async Task<bool> Open()
        {
            try
            {
                bool result = false;
                result =  await this.mcProtocolBinary.Open();
                return result;
            }
            catch (Exception ex)
            {
                logger.Create("Open: " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public async Task<bool> ReadBit(ushort startAddress, string DeviceName = "M")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadBit input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.mcProtocolBinary.ReadBit(DeviceName, startAddress);
            }
            finally
            {
                modbusSemaphore.Release();
            }
        }

        public async Task<List<bool>> ReadMultiBits(ushort startAddress, ushort quantity, string DeviceName = "M")
        {
            await modbusSemaphore.WaitAsync();
            var defaults = new List<bool>();
            if (startAddress > 65535)
            {
                logger.Create("ReadMultiBits input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                for (int i = 0; i < quantity; i++)
                {
                    defaults.Add(false);
                }
                return defaults;
            }
            if (quantity > 7168)
            {
                logger.Create("ReadMultiBits input lengh is exceed the allowable limit, lengh should be less than 2000.", LogLevel.Warning);
                for (int i = 0; i < quantity; i++)
                {
                    defaults.Add(false);
                }
                return defaults;
            }
            try
            {
                return await this.mcProtocolBinary.ReadMultiBits(DeviceName, startAddress, quantity);
            }
            finally
            {
                modbusSemaphore.Release();
            }
        }

        public async Task<int[]> ReadMultiSignedDWords(ushort startAddress, ushort count, string DeviceName = "D")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadMultiSignedDWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return Enumerable.Repeat((int)0, count).ToArray();
                }
                if (count > 4000)
                {
                    logger.Create("ReadMultiSignedDWords input lengh is exceed the allowable limit, lengh should be less than 62.", LogLevel.Warning);
                    return Enumerable.Repeat((int)0, count).ToArray();
                }
                return await this.mcProtocolBinary.ReadMultiSignedDWords(DeviceName, startAddress, count);
            }
            finally
            {
                modbusSemaphore.Release();
            }
        }

        public async Task<short[]> ReadMultiSignedWords(ushort startAddress, ushort length, string DeviceName = "D")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadMultiSignedWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }
                if (length > 7168)
                {
                    logger.Create("ReadMultiSignedWords input lengh is exceed the allowable limit, lengh should be less than 125.", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }
                return await this.mcProtocolBinary.ReadMultiSignedWords(DeviceName, startAddress, length);
            }
            finally
            {
                modbusSemaphore.Release();
            }
        }

        public async Task<int> ReadSignedDWord(ushort startAddress, string DeviceName = "D")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadSignedDWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return 0;
                }
                return await this.mcProtocolBinary.ReadSignedDWord(DeviceName, startAddress);
            }
            finally
            {
                modbusSemaphore.Release();
            }
        }

        public async Task<short> ReadSignedWord(ushort startAddress, string DeviceName = "D")
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                if (startAddress > 65535)
                {
                    logger.Create("ReadSignedWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return 0;
                }
                return await this.mcProtocolBinary.ReadSignedWord(DeviceName, startAddress);
            }
            finally
            {
                modbusSemaphore.Release();
            }
        }

        public async Task<bool> WriteBit(ushort address, bool value, string DeviceName = "M")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (address > 65535)
                {
                    logger.Create("WriteBit input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.mcProtocolBinary.WriteBit(DeviceName, address, value);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }

        public async Task<bool> WriteMultiBits(ushort address, bool[] values, string DeviceName = "M")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (address > 65535)
                {
                    logger.Create("WriteMultiBits input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                if (values.Length > 7168)
                {
                    logger.Create("WriteMultiBits input values is exceed the allowable limit, values should be less than 1968.", LogLevel.Warning);
                    return false;
                }
                if (values.Length % 2 != 0)
                {
                    List<bool> valuesNew = values.ToList();
                    valuesNew.Add(false);
                    return await this.mcProtocolBinary.WriteMultiBits(DeviceName, address, valuesNew.ToArray());
                }    
                return await this.mcProtocolBinary.WriteMultiBits(DeviceName, address, values);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }

        public async Task<bool> WriteMultiDWords(ushort addresses, int[] values, string DeviceName = "D")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (addresses > 65535)
                {
                    logger.Create("WriteMultiDWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                if (values.Length > 4000)
                {
                    logger.Create("WriteMultiDWords input values is exceed the allowable limit, values should be less than 60.", LogLevel.Warning);
                    return false;
                }
                return await this.mcProtocolBinary.WriteMultiDWords(DeviceName, addresses, values);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }

        public async Task<bool> WriteMultiWords(ushort addresses, short[] values, string DeviceName = "D")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (addresses > 65535)
                {
                    logger.Create("WriteMultiWords input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                if (values.Length > 7168)
                {
                    logger.Create("WriteMultiWords input values is exceed the allowable limit, values should be less than 120.", LogLevel.Warning);
                    return false;
                }
                return await this.mcProtocolBinary.WriteMultiWords(DeviceName, addresses, values);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }

        public async Task<bool> WriteSignedDWord(ushort address, int value, string DeviceName = "D")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (address > 65535)
                {
                    logger.Create("WriteSignedDWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.mcProtocolBinary.WriteSignedDWord(DeviceName, address, value);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }

        public async Task<bool> WriteSignedWord(ushort address, short value, string DeviceName = "D")
        {
            SystemsManager.Instance.isWriteDevice = true;
            await modbusSemaphore.WaitAsync();
            try
            {
                if (address > 65535)
                {
                    logger.Create("WriteSignedWord input startAddress is exceed the allowable limit, startAddress should be less than 65535.", LogLevel.Warning);
                    return false;
                }
                return await this.mcProtocolBinary.WriteSignedWord(DeviceName, address, value);
            }
            finally
            {
                SystemsManager.Instance.isWriteDevice = false;
                modbusSemaphore.Release();
            }
        }
    }
}
