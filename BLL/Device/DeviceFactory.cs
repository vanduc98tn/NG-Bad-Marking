using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Threading;
using System.Text.RegularExpressions;

namespace BLL
{
    public class DeviceFactory
    {
        private LoggerDebug logger = new LoggerDebug("DeviceFactory");

        public Device Device;

        private object obj = new object();
        private CancellationTokenSource monitorCancellation;

        //Notify.
        private NotifyPLCBits notifyPLCBits;
        private NotifyPLCWord notifyPLCWord;
        private NotifyPLCDWord notifyPLCDWord;

        public List<ushort> AddressDeviceWord_D = new List<ushort>();
        public Dictionary<string, short> monitorDeviceWord_D = new Dictionary<string, short>();

        public List<ushort> AddressDeviceDWord_D = new List<ushort>();
        public Dictionary<string, int> monitorDeviceDWord_D = new Dictionary<string, int>();

        public List<DeviceItem> AddressDeviceBits_M = new List<DeviceItem>();
        public Dictionary<string, bool> monitorDeviceBits_M = new Dictionary<string, bool>();

        public List<DeviceItem> AddressDeviceBits_X = new List<DeviceItem>();
        public Dictionary<string, bool> monitorDeviceBits_X = new Dictionary<string, bool>();

        public List<DeviceItem> AddressDeviceBits_Y = new List<DeviceItem>();
        public Dictionary<string, bool> monitorDeviceBits_Y = new Dictionary<string, bool>();

        public List<DeviceItem> AddressDeviceBits_L = new List<DeviceItem>();
        public Dictionary<string, bool> monitorDeviceBits_L = new Dictionary<string, bool>();

        Dictionary<string, bool> previousStateBits_M = new Dictionary<string, bool>();
        Dictionary<string, bool> previousStateBits_X = new Dictionary<string, bool>();
        Dictionary<string, bool> previousStateBits_Y = new Dictionary<string, bool>();
        Dictionary<string, bool> previousStateBits_L = new Dictionary<string, bool>();
        Dictionary<string, short> previousStateWord = new Dictionary<string, short>();
        Dictionary<string, int> previousStateDWord = new Dictionary<string, int>();

        private int limitDeviceBits;
        private int limitDeviceWord;

        public DeviceFactory(DeviceType deviceType,SettingDevice settingDevice)
        {
            this.LoadNotifyPLC();
            if (DeviceType.ModbusRTU == deviceType)
            {
                this.limitDeviceBits = 300;
                this.limitDeviceWord = 120;
                this.Device = new ServiceModbusRTUCOM(DeviceLength.PLC_LS_Modbus, settingDevice.modbusComSetting);
            }
            else if(DeviceType.ModbusTCP == deviceType)
            {
                this.limitDeviceWord = 120;
                this.limitDeviceBits = 2000;
                this.Device = new ServiceModbusTCP(DeviceLength.PLC_LS_Modbus, settingDevice.modbusTCPSetting);
            }
            else if(DeviceType.MCProtocolBinaryEthernet == deviceType)
            {
                this.limitDeviceWord = 7000;
                this.limitDeviceBits = 7000;
                this.Device = new ServiceMCProtocolBinaryTCP(DeviceLength.PLC_LS_Modbus, settingDevice.MCEProtocolBinarySetting);
            }
        }
        private void LoadNotifyPLC()
        {
            this.notifyPLCBits = SystemsManager.Instance.NotifyPLCBits;
            this.notifyPLCWord = SystemsManager.Instance.NotifyPLCWord;
            this.notifyPLCDWord = SystemsManager.Instance.NotifyPLCDWord;
        }
        public async void MonitorDevice()
        {
            this.monitorCancellation = new CancellationTokenSource();
            this.AddressDeviceWord_D = new List<ushort>();
            this.AddressDeviceDWord_D = new List<ushort>();
            this.AddressDeviceBits_M = new List<DeviceItem>();
            this.AddressDeviceBits_X = new List<DeviceItem>();
            this.AddressDeviceBits_Y = new List<DeviceItem>();
            this.AddressDeviceBits_L = new List<DeviceItem>();

            while (!this.monitorCancellation.Token.IsCancellationRequested)
            {
                if (!SystemsManager.Instance.isWriteDevice)
                {
                    await MonitorDeviceBits_M();
                    if(SystemsManager.Instance.AppSettings.CurrentDeviceType == DeviceType.MCProtocolBinaryEthernet)
                    {
                        await MonitorDeviceBits_X();
                        await MonitorDeviceBits_Y();
                        await MonitorDeviceBits_L();
                    }
                    await MonitorDeviceWord();
                    await MonitorDeviceDWord();
                }
                await Task.Delay(1);
            }
        }
        

        public void NotUseDevice()
        {
            this.monitorCancellation?.Cancel();
            this.Device.Close();
        }

        public async Task<short> ReadWordData(string Device)
        {
            short value;
            this.monitorDeviceWord_D.TryGetValue(Device, out value);
            await Task.Delay(1);
            return value;
        }
        private async Task MonitorDeviceWord()
        {
            var addressDeviceClone = this.AddressDeviceWord_D.ToList();
            this.previousStateWord = new Dictionary<string, short>(this.monitorDeviceWord_D);
            var monitorDeviceClone = new Dictionary<string, short>(this.monitorDeviceWord_D);
            if (BLLManager.Instance.DeviceLoadWord)
            {
                this.previousStateWord = new Dictionary<string, short>();
                foreach (var x in this.monitorDeviceWord_D)
                {
                    this.previousStateWord.Add(x.Key, 0);
                }
                BLLManager.Instance.DeviceLoadWord = false;
            }
            while (addressDeviceClone.Count > 0 && !BLLManager.Instance.DeviceLoadWord)
            {
                ushort addressMin = addressDeviceClone.Min();
                ushort addressMax = addressDeviceClone.Max();

                int lengthLm = (addressMax + 1) - addressMin;
                if (lengthLm <= this.limitDeviceWord)
                {
                    if (lengthLm == 0)
                    {
                        lengthLm = 1;
                    }
                    var values = await this.Device.ReadMultiSignedWords(addressMin, (ushort)lengthLm,"D");
                    for (int i = 0; i < values.Length; i++)
                    {
                        int addressNew = addressMin + i;
                        if (monitorDeviceClone.ContainsKey(addressNew.ToString()))
                        {
                            monitorDeviceClone[addressNew.ToString()] = values[i];
                            this.monitorDeviceWord_D[addressNew.ToString()] = values[i];
                        }
                    }
                    CheckChangedWord(this.previousStateWord, monitorDeviceClone);
                    break;
                }
                List<ushort> addressDeviceLmUp = addressDeviceClone.Where(x => x >= addressMin && x <= (addressMin + this.limitDeviceWord)).ToList();
                ushort addressMinLm = addressDeviceLmUp.Min();
                ushort addressMaxLm = addressDeviceLmUp.Max();
                int lengthLm1 = (addressMaxLm + 1) - addressMinLm;
                var values1 = await this.Device.ReadMultiSignedWords(addressMinLm, (ushort)(lengthLm1),"D");

                for (int i = 0; i < values1.Length; i++)
                {
                    int addressNew = addressMinLm + i;
                    if (monitorDeviceClone.ContainsKey(addressNew.ToString()))
                    {
                        monitorDeviceClone[addressNew.ToString()] = values1[i];
                        this.monitorDeviceWord_D[addressNew.ToString()] = values1[i];
                    }
                }
                addressDeviceClone.RemoveAll(x => x >= addressMin && x <= (addressMin + this.limitDeviceWord));
                CheckChangedWord(this.previousStateWord, monitorDeviceClone);
            }
        }
        public void AddAddressDeviceWord_D(ushort addressDevice)
        {
            BLLManager.Instance.DeviceLoadWord = true;
            if (this.AddressDeviceWord_D == null)
            {
                this.AddressDeviceWord_D = new List<ushort>();
                this.monitorDeviceWord_D = new Dictionary<string, short>();
            }
            if (this.AddressDeviceWord_D.Contains(addressDevice)) return;
            this.previousStateWord = new Dictionary<string, short>();
            this.AddressDeviceWord_D.Add(addressDevice);
            if (this.monitorDeviceWord_D.Any(x => x.Key == addressDevice.ToString())) return;
            this.monitorDeviceWord_D.Add(addressDevice.ToString(), 0);
            foreach(var x in this.monitorDeviceWord_D)
            {
                this.previousStateWord.Add(x.Key, 0);
            }
        }
        public void RemoveAddressDeviceWord_D(ushort addressDevice)
        {
            if (this.AddressDeviceWord_D == null)
            {
                this.AddressDeviceWord_D = new List<ushort>();
                this.monitorDeviceWord_D = new Dictionary<string, short>();
            }
            if (!this.AddressDeviceWord_D.Contains(addressDevice)) return;
            this.AddressDeviceWord_D.Remove(addressDevice);
            this.monitorDeviceWord_D.Remove(addressDevice.ToString());
        }

        public async Task<int> ReadDWordData(string Device)
        {
            int value;
            this.monitorDeviceDWord_D.TryGetValue(Device, out value);
            await Task.Delay(1);
            return value;
        }
        private async Task MonitorDeviceDWord()
        {
            var addressDeviceClone = this.AddressDeviceDWord_D.ToList();
            this.previousStateDWord = new Dictionary<string, int>(this.monitorDeviceDWord_D);
            var monitorDeviceClone = new Dictionary<string, int>(this.monitorDeviceDWord_D);
            if (BLLManager.Instance.DeviceLoadDWord)
            {
                this.previousStateDWord = new Dictionary<string, int>();
                foreach (var x in this.monitorDeviceDWord_D)
                {
                    this.previousStateDWord.Add(x.Key, 0);
                }
                BLLManager.Instance.DeviceLoadDWord = false;
            }
            while (addressDeviceClone.Count > 0 && !BLLManager.Instance.DeviceLoadDWord)
            {
                ushort addressMin = addressDeviceClone.Min();
                ushort addressMax = addressDeviceClone.Max();

                int lengthLm = (addressMax + 1) - addressMin;
                if (lengthLm <= (this.limitDeviceWord/2))
                {
                    if (lengthLm == 0)
                    {
                        lengthLm = 1;
                    }
                    var values = await this.Device.ReadMultiSignedDWords(addressMin, (ushort)lengthLm,"D");
                    
                    int count = 0;
                    for (int i = 0; i < values.Length; i++)
                    {
                        int addressNew = addressMin + count;
                        count += 2;
                        if (monitorDeviceClone.ContainsKey(addressNew.ToString()))
                        {
                            monitorDeviceClone[addressNew.ToString()] = values[i];
                            this.monitorDeviceDWord_D[addressNew.ToString()] = values[i];
                        }
                    }
                    CheckChangedDWord(this.previousStateDWord, monitorDeviceClone);
                    break;
                }
                else
                {
                    List<ushort> addressDeviceLmUp = addressDeviceClone.Where(x => x >= addressMin && x <= (addressMin + (this.limitDeviceWord / 2))).ToList();
                    ushort addressMinLm = addressDeviceLmUp.Min();
                    ushort addressMaxLm = addressDeviceLmUp.Max();
                    int lengthLm1 = (addressMaxLm + 1) - addressMinLm;
                    var values1 = await this.Device.ReadMultiSignedDWords(addressMinLm, (ushort)(lengthLm1), "D");
                    int count1 = 0;
                    for (int i = 0; i < values1.Length; i++)
                    {
                        int addressNew = addressMinLm + count1;
                        count1 += 2;
                        if (monitorDeviceClone.ContainsKey(addressNew.ToString()))
                        {
                            monitorDeviceClone[addressNew.ToString()] = values1[i];
                            this.monitorDeviceDWord_D[addressNew.ToString()] = values1[i];
                        }
                    }
                    addressDeviceClone.RemoveAll(x => x >= addressMin && x <= (addressMin + (this.limitDeviceWord / 2)));
                    CheckChangedDWord(this.previousStateDWord, monitorDeviceClone);
                }
            }
        }
        public void AddAddressDeviceDWord_D(ushort addressDevice)
        {
            try
            {
                BLLManager.Instance.DeviceLoadDWord = true;
                if (this.AddressDeviceDWord_D == null)
                {
                    this.AddressDeviceDWord_D = new List<ushort>();
                    this.monitorDeviceDWord_D = new Dictionary<string, int>();
                }
                if (this.AddressDeviceDWord_D.Contains(addressDevice)) return;
                this.previousStateDWord = new Dictionary<string, int>();
                
                this.AddressDeviceDWord_D.Add(addressDevice);
                if (this.monitorDeviceDWord_D.Any(x => x.Key == addressDevice.ToString())) return;
                this.monitorDeviceDWord_D.Add(addressDevice.ToString(), 0);
                foreach (var x in this.monitorDeviceDWord_D)
                {
                    this.previousStateDWord.Add(x.Key, 0);
                }
            }
            catch(Exception ex)
            {
                logger.Create("AddAddressDeviceDWord_D: " + ex.Message, LogLevel.Error);
            }
        }
        public void RemoveAddressDeviceDWord_D(ushort addressDevice)
        {
            if (this.AddressDeviceDWord_D == null)
            {
                this.AddressDeviceDWord_D = new List<ushort>();
                this.monitorDeviceDWord_D = new Dictionary<string, int>();
            }
            if (!this.AddressDeviceDWord_D.Contains(addressDevice)) return;
            this.AddressDeviceDWord_D.Remove(addressDevice);
            this.monitorDeviceDWord_D.Remove(addressDevice.ToString());
        }

        public async Task<bool> ReadBitsData_M(string Device)
        {
            bool value;
            this.monitorDeviceBits_M.TryGetValue(Device, out value);
            await Task.Delay(1);
            return value;
        }
        private async Task MonitorDeviceBits_M()
        {
            try
            {
                var addressDeviceClone = this.AddressDeviceBits_M.ToList();

                var monitorDeviceClone = new Dictionary<string, bool>(this.monitorDeviceBits_M);
                this.previousStateBits_M = new Dictionary<string, bool>(this.monitorDeviceBits_M);
                if(BLLManager.Instance.DeviceLoadBit_M)
                {
                    this.previousStateBits_M = new Dictionary<string, bool>();
                    foreach(var x in this.monitorDeviceBits_M)
                    {
                        this.previousStateBits_M.Add(x.Key, false);
                    }
                    BLLManager.Instance.DeviceLoadBit_M = false;
                }

                while (addressDeviceClone.Count > 0 && !BLLManager.Instance.DeviceLoadBit_M)
                {   
                    ushort addressMin = addressDeviceClone.Min(x => x.address);
                    ushort addressMax = addressDeviceClone.Max(x => x.address);
                    int lengthLm = (addressMax + 1) - addressMin;
                    if (lengthLm <= this.limitDeviceBits)
                    {
                        if (lengthLm == 0)
                        {
                            lengthLm = 1;
                        }
                        var values = await this.Device.ReadMultiBits(addressMin, (ushort)lengthLm, "M");
                        for (int i = 0; i < values.Count; i++)
                        {
                            int addressNew = addressMin + i;
                            if (monitorDeviceClone.ContainsKey("M" + addressNew.ToString()))
                            {
                                monitorDeviceClone["M" + addressNew.ToString()] = values[i];
                                this.monitorDeviceBits_M["M" + addressNew.ToString()] = values[i];
                            }
                        }
                        CheckChangedBits(previousStateBits_M, monitorDeviceClone);
                        break;
                    }
                    List<DeviceItem> addressDeviceLmUp = addressDeviceClone.Where(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits)).ToList();
                    ushort addressMinLm = addressDeviceLmUp.Min(x => x.address);
                    ushort addressMaxLm = addressDeviceLmUp.Max(x => x.address);
                    int lengthLm1 = (addressMaxLm + 1) - addressMinLm;
                    var values1 = await this.Device.ReadMultiBits(addressMinLm, (ushort)(lengthLm1), "M");
                    for (int i = 0; i < values1.Count; i++)
                    {
                        int addressNew = addressMinLm + i;
                        if (monitorDeviceClone.ContainsKey("M" + addressNew.ToString()))
                        {
                            monitorDeviceClone["M" + addressNew.ToString()] = values1[i];
                            this.monitorDeviceBits_M["M" + addressNew.ToString()] = values1[i];
                        }
                    }
                    addressDeviceClone.RemoveAll(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits));
                    CheckChangedBits(previousStateBits_M, monitorDeviceClone);
                }
            }
            catch(Exception ex)
            {
                logger.Create("MonitorDeviceBits_M: " + ex.Message, LogLevel.Error);
            }
            
        }
        private void AddAddressDeviceBits_M(string deviceType, ushort addressDevice)
        {
            BLLManager.Instance.DeviceLoadBit_M = true;
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_M == null)
            {
                this.AddressDeviceBits_M = new List<DeviceItem>();
                this.monitorDeviceBits_M = new Dictionary<string, bool>();
            }
            if (this.AddressDeviceBits_M.Any(x=>x.address== addressDevice)) return;
            this.previousStateBits_M = new Dictionary<string, bool>();
            this.AddressDeviceBits_M.Add(item);
            if (this.monitorDeviceBits_M.Any(x => x.Key == deviceType+ addressDevice.ToString())) return;
            this.monitorDeviceBits_M.Add(item.type + item.address, false);
            this.ClearPreviousStateBits(this.previousStateBits_M, this.monitorDeviceBits_M);
        }
        private void RemoveAddressDeviceBits_M(string deviceType, ushort addressDevice)
        {
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_M == null)
            {
                this.AddressDeviceBits_M = new List<DeviceItem>();
                this.monitorDeviceBits_M = new Dictionary<string, bool>();
            }
            if (!this.AddressDeviceBits_M.Any(x => x.address == addressDevice)) return;
            int indexToRemove = AddressDeviceBits_M.FindIndex(x => x.address == addressDevice);
            if (indexToRemove != -1)
            {
                AddressDeviceBits_M.RemoveAt(indexToRemove);
            }
            this.AddressDeviceBits_M.Remove(item);
            this.monitorDeviceBits_M.Remove(item.type + item.address);
        }

        public async Task<bool> ReadBitsData_X(string Device)
        {
            bool value;
            this.monitorDeviceBits_X.TryGetValue(Device, out value);
            await Task.Delay(1);
            return value;
        }
        private async Task MonitorDeviceBits_X()
        {
            var addressDeviceClone = this.AddressDeviceBits_X.ToList();
            this.previousStateBits_X = new Dictionary<string, bool>(this.monitorDeviceBits_X);
            var monitorDeviceClone = new Dictionary<string, bool>(this.monitorDeviceBits_X);
            if (BLLManager.Instance.DeviceLoadBit_X)
            {
                this.previousStateBits_X = new Dictionary<string, bool>();
                foreach (var x in this.monitorDeviceBits_X)
                {
                    this.previousStateBits_X.Add(x.Key, false);
                }
                BLLManager.Instance.DeviceLoadBit_X = false;
            }
            while (addressDeviceClone.Count > 0 && !BLLManager.Instance.DeviceLoadBit_X)
            {
                ushort addressMin = addressDeviceClone.Min(x => x.address);
                ushort addressMax = addressDeviceClone.Max(x => x.address);

                int lengthLm = (addressMax + 1) - addressMin;
                if (lengthLm <= this.limitDeviceBits)
                {
                    if (lengthLm == 0)
                    {
                        lengthLm = 1;
                    }
                    var values = await this.Device.ReadMultiBits(addressMin, (ushort)lengthLm, "X");
                    for (int i = 0; i < values.Count; i++)
                    {
                        int addressNew = addressMin + i;
                        if (monitorDeviceClone.ContainsKey("X"+addressNew.ToString()))
                        {
                            monitorDeviceClone["X" + addressNew.ToString()] = values[i];
                            this.monitorDeviceBits_X["X" + addressNew.ToString()] = values[i];
                        }
                    }
                    CheckChangedBits(this.previousStateBits_X, monitorDeviceClone);
                    break;
                }
                List<DeviceItem> addressDeviceLmUp = addressDeviceClone.Where(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits)).ToList();
                ushort addressMinLm = addressDeviceLmUp.Min(x => x.address);
                ushort addressMaxLm = addressDeviceLmUp.Max(x => x.address);
                int lengthLm1 = (addressMaxLm + 1) - addressMinLm;
                var values1 = await this.Device.ReadMultiBits(addressMinLm, (ushort)(lengthLm1));

                for (int i = 0; i < values1.Count; i++)
                {
                    int addressNew = addressMinLm + i;
                    if (monitorDeviceClone.ContainsKey("X" + addressNew.ToString()))
                    {
                        monitorDeviceClone["X" + addressNew.ToString()] = values1[i];
                        this.monitorDeviceBits_X["X" + addressNew.ToString()] = values1[i];
                    }
                }
                addressDeviceClone.RemoveAll(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits));
                CheckChangedBits(this.previousStateBits_X, monitorDeviceClone);
            }
        }
        private void AddAddressDeviceBits_X(string deviceType,ushort addressDevice)
        {
            DeviceItem item = new DeviceItem();
            BLLManager.Instance.DeviceLoadBit_X = true;
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_X == null)
            {
                this.AddressDeviceBits_X = new List<DeviceItem>();
                this.monitorDeviceBits_X = new Dictionary<string, bool>();
            }
            if (this.AddressDeviceBits_X.Any(x => x.address == addressDevice)) return;
            this.previousStateBits_X = new Dictionary<string, bool>();
            this.AddressDeviceBits_X.Add(item);
            if (this.monitorDeviceBits_X.Any(x => x.Key == deviceType + addressDevice.ToString())) return;
            this.monitorDeviceBits_X.Add(item.type+ item.address, false);
            this.ClearPreviousStateBits(this.previousStateBits_X, this.monitorDeviceBits_X);
        }
        private void RemoveAddressDeviceBits_X(string deviceType, ushort addressDevice)
        {
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_X == null)
            {
                this.AddressDeviceBits_X = new List<DeviceItem>();
                this.monitorDeviceBits_X = new Dictionary<string, bool>();
            }
            if (!this.AddressDeviceBits_X.Any(x => x.address == addressDevice)) return;
            int indexToRemove = AddressDeviceBits_X.FindIndex(x => x.address == addressDevice);
            if (indexToRemove != -1)
            {
                AddressDeviceBits_X.RemoveAt(indexToRemove);
            }
            this.monitorDeviceBits_X.Remove(item.type + item.address);
        }

        public async Task<bool> ReadBitsData_Y(string Device)
        {
            bool value;
            this.monitorDeviceBits_Y.TryGetValue(Device, out value);
            await Task.Delay(1);
            return value;
        }
        private async Task MonitorDeviceBits_Y()
        {
            var addressDeviceClone = this.AddressDeviceBits_Y.ToList();
            this.previousStateBits_Y = new Dictionary<string, bool>(this.monitorDeviceBits_Y);
            var monitorDeviceClone = new Dictionary<string, bool>(this.monitorDeviceBits_Y);
            if (BLLManager.Instance.DeviceLoadBit_Y)
            {
                this.previousStateBits_Y = new Dictionary<string, bool>();
                foreach (var x in this.monitorDeviceBits_Y)
                {
                    this.previousStateBits_Y.Add(x.Key, false);
                }
                BLLManager.Instance.DeviceLoadBit_Y = false;
            }
            while (addressDeviceClone.Count > 0 && !BLLManager.Instance.DeviceLoadBit_Y)
            {
                ushort addressMin = addressDeviceClone.Min(x => x.address);
                ushort addressMax = addressDeviceClone.Max(x => x.address);

                int lengthLm = (addressMax + 1) - addressMin;
                if (lengthLm <= this.limitDeviceBits)
                {
                    if (lengthLm == 0)
                    {
                        lengthLm = 1;
                    }
                    var values = await this.Device.ReadMultiBits(addressMin, (ushort)lengthLm, "Y");
                    for (int i = 0; i < values.Count; i++)
                    {
                        int addressNew = addressMin + i;
                        if (monitorDeviceClone.ContainsKey("Y" + addressNew.ToString()))
                        {
                            monitorDeviceClone["Y" + addressNew.ToString()] = values[i];
                            this.monitorDeviceBits_Y["Y" + addressNew.ToString()] = values[i];
                        }
                    }
                    CheckChangedBits(this.previousStateBits_Y, monitorDeviceClone);
                    break;
                }
                List<DeviceItem> addressDeviceLmUp = addressDeviceClone.Where(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits)).ToList();
                ushort addressMinLm = addressDeviceLmUp.Min(x => x.address);
                ushort addressMaxLm = addressDeviceLmUp.Max(x => x.address);
                int lengthLm1 = (addressMaxLm + 1) - addressMinLm;
                var values1 = await this.Device.ReadMultiBits(addressMinLm, (ushort)(lengthLm1));

                for (int i = 0; i < values1.Count; i++)
                {
                    int addressNew = addressMinLm + i;
                    if (monitorDeviceClone.ContainsKey("Y" + addressNew.ToString()))
                    {
                        monitorDeviceClone["Y" + addressNew.ToString()] = values1[i];
                        this.monitorDeviceBits_Y["Y" + addressNew.ToString()] = values1[i];
                    }
                }
                addressDeviceClone.RemoveAll(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits));
                CheckChangedBits(this.previousStateBits_Y, monitorDeviceClone);
            }
        }
        private void AddAddressDeviceBits_Y(string deviceType, ushort addressDevice)
        {
            BLLManager.Instance.DeviceLoadBit_Y = true;
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_Y == null)
            {
                this.AddressDeviceBits_Y = new List<DeviceItem>();
                this.monitorDeviceBits_Y = new Dictionary<string, bool>();
            }
            if (this.AddressDeviceBits_Y.Any(x => x.address == addressDevice)) return;
            this.previousStateBits_Y = new Dictionary<string, bool>();
            this.AddressDeviceBits_Y.Add(item);
            if (this.monitorDeviceBits_Y.Any(x => x.Key == deviceType + addressDevice.ToString())) return;
            this.monitorDeviceBits_Y.Add(item.type + item.address, false);
            this.ClearPreviousStateBits(this.previousStateBits_Y, this.monitorDeviceBits_Y);
        }
        private void RemoveAddressDeviceBits_Y(string deviceType, ushort addressDevice)
        {
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_Y == null)
            {
                this.AddressDeviceBits_Y = new List<DeviceItem>();
                this.monitorDeviceBits_Y = new Dictionary<string, bool>();
            }
            if (!this.AddressDeviceBits_Y.Any(x => x.address == addressDevice)) return;
            int indexToRemove = AddressDeviceBits_Y.FindIndex(x => x.address == addressDevice);
            if (indexToRemove != -1)
            {
                AddressDeviceBits_Y.RemoveAt(indexToRemove);
            }
            this.monitorDeviceBits_Y.Remove(item.type + item.address);
        }

        public async Task<bool> ReadBitsData_L(string Device)
        {
            bool value;
            this.monitorDeviceBits_L.TryGetValue(Device, out value);
            await Task.Delay(1);
            return value;
        }
        private async Task MonitorDeviceBits_L()
        {
            var addressDeviceClone = this.AddressDeviceBits_L.ToList();
            this.previousStateBits_L = new Dictionary<string, bool>(this.monitorDeviceBits_L);
            var monitorDeviceClone = new Dictionary<string, bool>(this.monitorDeviceBits_L);
            if (BLLManager.Instance.DeviceLoadBit_L)
            {
                this.previousStateBits_L = new Dictionary<string, bool>();
                foreach (var x in this.monitorDeviceBits_L)
                {
                    this.previousStateBits_L.Add(x.Key, false);
                }
                BLLManager.Instance.DeviceLoadBit_L = false;
            }
            while (addressDeviceClone.Count > 0 && !BLLManager.Instance.DeviceLoadBit_L)
            {
                ushort addressMin = addressDeviceClone.Min(x => x.address);
                ushort addressMax = addressDeviceClone.Max(x => x.address);

                int lengthLm = (addressMax + 1) - addressMin;
                if (lengthLm <= this.limitDeviceBits)
                {
                    if (lengthLm == 0)
                    {
                        lengthLm = 1;
                    }
                    var values = await this.Device.ReadMultiBits(addressMin, (ushort)lengthLm, "L");
                    for (int i = 0; i < values.Count; i++)
                    {
                        int addressNew = addressMin + i;
                        if (monitorDeviceClone.ContainsKey("L" + addressNew.ToString()))
                        {
                            monitorDeviceClone["L" + addressNew.ToString()] = values[i];
                            this.monitorDeviceBits_L["L" + addressNew.ToString()] = values[i];
                        }
                    }
                    CheckChangedBits(this.previousStateBits_L, monitorDeviceClone);
                    break;
                }
                List<DeviceItem> addressDeviceLmUp = addressDeviceClone.Where(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits)).ToList();
                ushort addressMinLm = addressDeviceLmUp.Min(x => x.address);
                ushort addressMaxLm = addressDeviceLmUp.Max(x => x.address);
                int lengthLm1 = (addressMaxLm + 1) - addressMinLm;
                var values1 = await this.Device.ReadMultiBits(addressMinLm, (ushort)(lengthLm1));

                for (int i = 0; i < values1.Count; i++)
                {
                    int addressNew = addressMinLm + i;
                    if (monitorDeviceClone.ContainsKey("L" + addressNew.ToString()))
                    {
                        monitorDeviceClone["L" + addressNew.ToString()] = values1[i];
                        this.monitorDeviceBits_L["L" + addressNew.ToString()] = values1[i];
                    }
                }
                addressDeviceClone.RemoveAll(x => x.address >= addressMin && x.address <= (addressMin + this.limitDeviceBits));
                CheckChangedBits(this.previousStateBits_L, monitorDeviceClone);
            }
        }
        private void AddAddressDeviceBits_L(string deviceType, ushort addressDevice)
        {
            BLLManager.Instance.DeviceLoadBit_L = true;
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_L == null)
            {
                this.AddressDeviceBits_L = new List<DeviceItem>();
                this.monitorDeviceBits_L = new Dictionary<string, bool>();
            }
            if (this.AddressDeviceBits_L.Any(x => x.address == addressDevice)) return;
            this.previousStateBits_L = new Dictionary<string, bool>();
            this.AddressDeviceBits_L.Add(item);
            if (this.monitorDeviceBits_L.Any(x => x.Key == deviceType + addressDevice.ToString())) return;
            this.monitorDeviceBits_L.Add(item.type + item.address, false);
            this.ClearPreviousStateBits(this.previousStateBits_L, this.monitorDeviceBits_L);
        }
        private void RemoveAddressDeviceBits_L(string deviceType, ushort addressDevice)
        {
            DeviceItem item = new DeviceItem();
            item.type = deviceType;
            item.address = addressDevice;
            if (this.AddressDeviceBits_L == null)
            {
                this.AddressDeviceBits_L = new List<DeviceItem>();
                this.monitorDeviceBits_L = new Dictionary<string, bool>();
            }
            if (!this.AddressDeviceBits_L.Any(x => x.address == addressDevice)) return;
            int indexToRemove = AddressDeviceBits_L.FindIndex(x => x.address == addressDevice);
            if (indexToRemove != -1)
            {
                AddressDeviceBits_L.RemoveAt(indexToRemove);
            }
            this.monitorDeviceBits_L.Remove(item.type + item.address);
        }

        private void CheckChangedBits(Dictionary<string, bool> previousState, Dictionary<string, bool> currentState)
        {
            try
            {
                List<string> changedKeys = GetChangedKeysBits(previousState, currentState);
                if (changedKeys.Count > 0)
                {
                    foreach (var key in changedKeys)
                    {
                        NotifyStatusChangedBits(key, currentState[key]);
                    }
                }
                // Cập nhật previousState
                previousState.Clear();
                foreach (var kvp in currentState)
                {
                    previousState[kvp.Key] = kvp.Value;
                }
            }
            catch(Exception ex)
            {
                logger.Create("CheckChangedBits:" + ex.Message, LogLevel.Error);
            }
        }
        private List<string> GetChangedKeysBits(Dictionary<string, bool> previousState, Dictionary<string, bool> currentState)
        {
            List<string> changedKeys = new List<string>();
            try
            {
                foreach (var kvp in currentState)
                {
                    if (previousState.TryGetValue(kvp.Key, out var prevValue) && kvp.Value != prevValue)
                    {
                        changedKeys.Add(kvp.Key);
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Create("GetChangedKeysBits: " + ex.Message, LogLevel.Error);
            }
            return changedKeys;
        }

        private void CheckChangedWord(Dictionary<string, short> previousState, Dictionary<string, short> currentState)
        {
            List<string> changedKeys = GetChangedKeysWord(previousState, currentState);
            if (changedKeys.Count > 0)
            {
                foreach (var key in changedKeys)
                {
                    NotifyStatusChangedWord(key, currentState[key]);
                }
            }
            // Cập nhật previousState
            previousState.Clear();
            foreach (var kvp in currentState)
            {
                previousState[kvp.Key] = kvp.Value;
            }
        }
        private List<string> GetChangedKeysWord(Dictionary<string, short> previousState, Dictionary<string, short> currentState)
        {
            List<string> changedKeys = new List<string>();

            foreach (var kvp in currentState)
            {
                if (previousState.TryGetValue(kvp.Key, out var prevValue) && kvp.Value != prevValue || kvp.Value == 0)
                {
                    changedKeys.Add(kvp.Key);
                }
            }
            return changedKeys;
        }

        private void CheckChangedDWord(Dictionary<string, int> previousState, Dictionary<string, int> currentState)
        {
            List<string> changedKeys = GetChangedKeysDWord(previousState, currentState);
            if (changedKeys.Count > 0)
            {
                foreach (var key in changedKeys)
                {
                    NotifyStatusChangedDWord(key, currentState[key]);
                }
            }
            // Cập nhật previousState
            previousState.Clear();
            foreach (var kvp in currentState)
            {
                previousState[kvp.Key] = kvp.Value;
            }
        }
        private List<string> GetChangedKeysDWord(Dictionary<string, int> previousState, Dictionary<string, int> currentState)
        {
            List<string> changedKeys = new List<string>();

            foreach (var kvp in currentState)
            {
                if (previousState.TryGetValue(kvp.Key, out var prevValue) && kvp.Value != prevValue || kvp.Value == 0)
                {
                    changedKeys.Add(kvp.Key);
                }
            }
            return changedKeys;
        }

        private void ClearPreviousStateBits(Dictionary<string, bool> previousState, Dictionary<string, bool> OrgState)
        {
            foreach (var x in OrgState)
            {
                previousState.Add(x.Key, false);
            }
        }


        public void AddBitAddress(string deviceName,ushort address)
        {
            if(deviceName == DeviceName.M.ToString())
            {
                this.AddAddressDeviceBits_M(deviceName.ToString(), address);
            }
            else if(deviceName == DeviceName.X.ToString())
            {
                this.AddAddressDeviceBits_X(deviceName.ToString(), address);
            }
            else if (deviceName == DeviceName.Y.ToString())
            {
                this.AddAddressDeviceBits_Y(deviceName.ToString(), address);
            }
            else if (deviceName == DeviceName.L.ToString())
            {
                this.AddAddressDeviceBits_L(deviceName.ToString(), address);
            }
        }
        public void RemoveBitAddress(string deviceName, ushort address)
        {
            if (deviceName == DeviceName.M.ToString())
            {
                this.RemoveAddressDeviceBits_M(deviceName.ToString(), address);
            }
            else if (deviceName == DeviceName.X.ToString())
            {
                this.RemoveAddressDeviceBits_X(deviceName.ToString(), address);
            }
            else if (deviceName == DeviceName.Y.ToString())
            {
                this.RemoveAddressDeviceBits_Y(deviceName.ToString(), address);
            }
            else if (deviceName == DeviceName.L.ToString())
            {
                this.RemoveAddressDeviceBits_L(deviceName.ToString(), address);
            }
        }

        private void NotifyStatusChangedBits(string key,bool status)
        {
            try
            {
                this.notifyPLCBits.NotifyChangeBits(key, status);
            }
            catch(Exception ex)
            {
                logger.Create("NotifyStatusChangedBits: " + ex.Message, LogLevel.Error);
            }
        }
        private void NotifyStatusChangedWord(string key, short value)
        {
            this.notifyPLCWord.NotifyChangeWord(key, value);
        }
        private void NotifyStatusChangedDWord(string key , int value)
        {
            this.notifyPLCDWord.NotifyChangeDWord(key, value);
        }
    }
    public class DeviceItem
    {
        public string type { get; set; }
        public ushort address { get; set; }
    }
}
