using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class MCProtocolBinary : BaseTCPClient
    {
        private LoggerDebug logger = new LoggerDebug("MCProtocolBinary");
        public MCProtocolBinary(string ip, int port) : base("MCProtocolBinary", ip, port)
        {
        }
        public async Task<bool> ReadBit(string deviceName, ushort startAddress)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadBit Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadBitRequest(deviceName, startAddress);
                await this.SendMCRequest(request);
                await WaitReturnData();
                return this.ParseReadBitResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadBit: " + ex.Message);
                logger.Create("ReadBit: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<List<bool>> ReadMultiBits(string DeviceName,ushort startAddress, ushort quantity)
        {
            List<bool> defaults = new List<bool>();
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadMultiBits Unsuccessful, Because Status Is Disconnectd!!!");
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadMultiBitsRequest(DeviceName, startAddress, quantity);
                await this.SendMCRequest(request);
                await WaitReturnData();
                return this.ParseReadMultiBitsResponse(this.txBufReceiver, quantity);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadMultiBits: " + ex.Message);
                logger.Create("ReadBit: " + ex.Message, LogLevel.Error);
            }
            for (int i = 0; i < quantity; i++)
            {
                defaults.Add(false);
            }
            return defaults;
        }
        public async Task<short[]> ReadMultiSignedWords(string DeviceName, ushort startAddress, ushort length)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return Enumerable.Repeat((short)0, length).ToArray();
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadMultiSignedWordsRequest(DeviceName, startAddress, length);
                await this.SendMCRequest(request);
                await WaitReturnData();
                return this.ParseReadMultiSignedWordsResponse(this.txBufReceiver, length);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedWords: " + ex.Message);
                logger.Create("ReadMultiSignedWords: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat((short)0, length).ToArray();
        }
        public async Task<short> ReadSignedWord(string deviceName, ushort startAddress)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadSignedWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return 0;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = BuildReadSignedWordRequest(deviceName, startAddress);
                await SendMCRequest(request);
                await WaitReturnData();
                return ParseReadSignedWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadSignedWord: " + ex.Message);
                logger.Create("ReadSignedWord: " + ex.Message, LogLevel.Error);
            }

            return 0; // Hoặc giá trị mặc định khác nếu có lỗi
        }
        public async Task<int[]> ReadMultiSignedDWords(string DeviceName, ushort startAddress, ushort length)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedDWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return Enumerable.Repeat((int)0, length).ToArray();
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadMultiSignedDWordsRequest(DeviceName, startAddress, length);
                await this.SendMCRequest(request);
                await WaitReturnData();
                return this.ParseReadMultiSignedDWordsResponse(this.txBufReceiver, length);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedDWords: " + ex.Message);
                logger.Create("ReadMultiSignedDWords: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat((int)0, length).ToArray();
        }
        public async Task<int> ReadSignedDWord(string deviceName, ushort startAddress)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadSignedDWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return 0;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = BuildReadSignedDWordRequest(deviceName, startAddress);
                await SendMCRequest(request);
                await WaitReturnData();
                return ParseReadSignedDWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadSignedDWord: " + ex.Message);
                logger.Create("ReadSignedDWord: " + ex.Message, LogLevel.Error);
            }

            return 0; // Hoặc giá trị mặc định khác nếu có lỗi
        }

        public async Task<bool> WriteBit(string deviceName, ushort address, bool value)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("WriteBit Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = BuildWriteBitRequest(deviceName, address, value);
                await SendMCRequest(request);
                await WaitReturnData();
                return ParseWriteBitResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("WriteBit: " + ex.Message);
                logger.Create("WriteBit: " + ex.Message, LogLevel.Error);
            }

            return false; // Hoặc giá trị mặc định khác nếu có lỗi
        }
        public async Task<bool> WriteSignedWord(string deviceName, ushort address, short value)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("WriteSignedWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = BuildWriteSignedWordRequest(deviceName, address, value);
                await SendMCRequest(request);
                await WaitReturnData();
                return ParseWriteSignedWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("WriteSignedWord: " + ex.Message);
                logger.Create("WriteSignedWord: " + ex.Message, LogLevel.Error);
            }

            return false; // Hoặc giá trị mặc định khác nếu có lỗi
        }
        public async Task<bool> WriteSignedDWord(string deviceName, ushort address, int value)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("WriteSignedDWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = BuildWriteSignedDWordRequest(deviceName, address, value);
                await SendMCRequest(request);
                await WaitReturnData();
                return ParseWriteSignedDWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("WriteSignedDWord: " + ex.Message);
                logger.Create("WriteSignedDWord: " + ex.Message, LogLevel.Error);
            }

            return false; // Hoặc giá trị mặc định khác nếu có lỗi
        }
        public async Task<bool> WriteMultiBits(string deviceName, ushort address, bool[] values)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("WriteMultiBits Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                if (values == null || values.Length == 0)
                {
                    logger.Create("WriteMultiBits: Values array is empty.", LogLevel.Warning);
                    return false;
                }

                this.isReceiver = false;
                byte[] request = BuildWriteMultiBitsRequest(deviceName, address, values);
                await SendMCRequest(request);
                await WaitReturnData();
                return ParseWriteMultiBitsResponse(this.txBufReceiver, values.Length);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("WriteMultiBits: " + ex.Message);
                logger.Create("WriteMultiBits: " + ex.Message, LogLevel.Error);
            }

            return false; // Hoặc giá trị mặc định khác nếu có lỗi
        }
        public async Task<bool> WriteMultiWords(string deviceName, ushort addresses, short[] values)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("WriteMultiWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteMultiWordsRequest(deviceName, addresses, values);
                await this.SendMCRequest(request);
                await WaitReturnData();
                return this.ParseWriteMultiWordsResponse(this.txBufReceiver, addresses);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("WriteMultiWords: " + ex.Message);
                logger.Create("WriteMultiWords: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> WriteMultiDWords(string deviceName, ushort addresses, int[] values)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("WriteMultiDWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteMultiDWordsRequest(deviceName, addresses, values);
                await this.SendMCRequest(request);
                await WaitReturnData();
                return this.ParseWriteMultiDWordsResponse(this.txBufReceiver, addresses, values.Length);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("WriteMultiDWords: " + ex.Message);
                logger.Create("WriteMultiDWords: " + ex.Message, LogLevel.Error);
            }
            return false;
        }



        private byte[] BuildReadBitRequest(string deviceName, ushort startAddress)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0C;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x04;
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = 0x01;
            request[20] = 0x00;
            return request;
        }
        private byte[] BuildReadMultiBitsRequest(string DeviceName,ushort startAddress, ushort quantity)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0C;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x04;
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(DeviceName, request);
            // Number Point
            request[19] = (byte)(quantity & 0xff);
            request[20] = (byte)(quantity >> 8);
            return request;
        }
        private byte[] BuildReadMultiSignedWordsRequest(string DeviceName, ushort startAddress, ushort length)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0C;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x04; // Subcommand for Read Multiple Words
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(DeviceName, request);
            // Number Point
            request[19] = (byte)(length & 0xff);
            request[20] = (byte)(length >> 8);
            return request;
        }
        private byte[] BuildReadSignedWordRequest(string deviceName, ushort startAddress)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0C;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x04; // Subcommand for Read Multiple Words
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = 0x01;
            request[20] = 0x00;
            return request;
        }
        private byte[] BuildReadMultiSignedDWordsRequest(string DeviceName, ushort startAddress, ushort count)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0C;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x04; // Subcommand for Read Multiple Signed Double Words
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(DeviceName, request);
            // Number Point
            request[19] = (byte)((count * 2) & 0xff);
            request[20] = (byte)((count * 2) >> 8);
            return request;
        }
        private byte[] BuildReadSignedDWordRequest(string deviceName, ushort startAddress)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0C;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x04; // Subcommand for Read Multiple Words
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = 0x02;
            request[20] = 0x00;
            return request;
        }
        private byte[] BuildWriteBitRequest(string deviceName, ushort startAddress, bool value)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[22];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0D;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x14;
            request[13] = 0x01;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = 0x01;
            request[20] = 0x00;
            // Value
            request[21] = (byte)(value ? 0x10 : 0x00);
            return request;
        }
        private byte[] BuildWriteSignedWordRequest(string deviceName, ushort startAddress, short value)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[23];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x0E;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x14;
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = 0x01;
            request[20] = 0x00;
            //Value write
            request[21] = (byte)(value & 0xff);
            request[22] = (byte)(value >> 8);
            return request;
        }
        private byte[] BuildWriteSignedDWordRequest(string deviceName, ushort startAddress, int value)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[25];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = 0x10;
            request[8] = 0x00;
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x14;
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = 0x02;
            request[20] = 0x00;
            //Value write
            request[21] = (byte)(value & 0xff);
            request[22] = (byte)(value >> 8);
            request[23] = (byte)(value >> 16);
            request[24] = (byte)(value >> 24);
            return request;
        }
        private byte[] BuildWriteMultiBitsRequest(string deviceName, ushort startAddress, bool[] values)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21 + (values.Length/2)];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = (Byte)((0x10 + (values.Length / 2) - 4) & 0xff);
            request[8] = (Byte)((0x10 + (values.Length / 2) - 4) >> 8);
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x14;
            request[13] = 0x01;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = (byte)(((values.Length / 2) * 2) & 0xFF);
            request[20] = (byte)(((values.Length / 2) * 2) >> 8);
            // Value
            int idex = 0;
            for (int i = 0; i < (values.Length / 2); i++)
            {
                // 11 10 01 00.
                byte high = 0x00;
                byte low = 0x00;

                high = (byte)(values[idex] ? 0x10 : 0x00);
                low = (byte)(values[idex + 1] ? 0x01 : 0x00);

                int result = high + low;
                request[21+ i] = (byte)(result & 0xFF);

                idex += 2;
            }    
            return request;
        }
        private byte[] BuildWriteMultiWordsRequest(string deviceName, ushort startAddress, short[] values)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21 + (values.Length * 2)];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = (Byte)((0x10 + (values.Length*2) - 4) & 0xff);
            request[8] = (Byte)((0x10 + (values.Length*2) - 4) >> 8);
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x14;
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = (Byte)((0x01 * values.Length) & 0xff);
            request[20] = (Byte)((0x01 * values.Length) >> 8);
            //Value write
            int idex = 0;
            for(int i = 0; i < values.Length;i++)
            {
                request[21 + (idex)] = (Byte)(values[i] & 0xff);
                request[21 + (idex+1)] = (Byte)(values[i] >> 8);
                idex += 2;
            }
            return request;
        }
        private byte[] BuildWriteMultiDWordsRequest(string deviceName, ushort startAddress, int[] values)
        {
            int monitoring_timer = 10;
            byte[] request = new byte[21 + (values.Length * 4)];
            request[0] = 0x50;
            request[1] = 0x00;
            request[2] = 0x00;
            request[3] = 0xFF;
            request[4] = 0xFF;
            request[5] = 0x03;
            request[6] = 0x00;
            request[7] = (Byte)((0x10 + (values.Length * 4) - 4) & 0xff);
            request[8] = (Byte)((0x10 + (values.Length * 4) - 4) >> 8);
            //Monitor Timer
            request[9] = (byte)(monitoring_timer & 0xff);
            request[10] = (byte)(monitoring_timer >> 8);
            //Command + Subcommand
            request[11] = 0x01;
            request[12] = 0x14;
            request[13] = 0x00;
            request[14] = 0x00;
            //Start Point
            request[15] = (byte)(startAddress & 0xff);
            request[16] = (byte)(startAddress >> 8);
            request[17] = (byte)(startAddress >> 16);
            this.SetDevice(deviceName, request);
            // Number Point
            request[19] = (Byte)((0x02 * values.Length) & 0xff);
            request[20] = (Byte)((0x02 * values.Length) >> 8);
            //Value write
            int idex = 0;
            for (int i = 0; i < values.Length; i++)
            {
                request[21 + (idex)] = (Byte)(values[i] & 0xff);
                request[21 + (idex + 1)] = (Byte)(values[i] >> 8);
                request[21 + (idex + 2)] = (Byte)(values[i] >> 16);
                request[21 + (idex + 3)] = (Byte)(values[i] >> 24);
                idex += 4;
            }
            return request;
        }



        private bool ParseReadBitResponse(byte[] response)
        {
            // TODO.
            if (response == null) return false;
            if (response[11] == 0x02) return false;
            if (response[11] == 0x03) return true;
            return false;
        }
        private List<bool> ParseReadMultiBitsResponse(byte[] response, ushort quantity)
        {
            var defaults = new List<bool>();
            try
            {
                if (response == null || response.Length < 5)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: Invalid response data");
                    logger.Create("ParseReadMultiBitsResponse: Invalid response data", LogLevel.Warning);
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                // SubHeader Error
                if (response[0] != 0xd0 || response[1] != 0x00)
                {
                    // SubHeader Error
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: SubHeader Error");
                    logger.Create("ParseReadMultiBitsResponse: SubHeader Error", LogLevel.Warning);
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                // Kiểm tra NetWord No.
                if (response[2] != 0x00)
                {
                    // Network No Error
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: Network No Error");
                    logger.Create("ParseReadMultiBitsResponse: Network No Error", LogLevel.Warning);
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                var bitValues = new List<bool>();
                for (int i = 0; i < quantity; i++)
                {
                    Int32 byteIdx = 11 + i / 8;
                    int bitPos = i % 8;
                    if ((response[byteIdx] & (1 << bitPos)) != 0)
                    {
                        bitValues.Add(true);
                    }
                    else
                    {
                        bitValues.Add(false);
                    }
                }
                return bitValues;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: " + ex.Message);
                logger.Create("ParseReadMultiBitsResponse: " + ex.Message, LogLevel.Error);
            }
            for (int i = 0; i < quantity; i++)
            {
                defaults.Add(false);
            }
            return defaults;
        }
        private short[] ParseReadMultiSignedWordsResponse(byte[] response, ushort length)
        {
            short[] defaults = new short[length];
            try
            {
                if (response == null || response.Length < 5)
                {
                    // Handle error or insufficient data message
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: Invalid response data");
                    logger.Create("ParseReadMultiSignedWordsResponse: Invalid response data", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                // Additional parsing logic based on your specific protocol

                // SubHeader Error
                if (response[0] != 0xd0 || response[1] != 0x00)
                {
                    // SubHeader Error
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: SubHeader Error");
                    logger.Create("ParseReadMultiSignedWordsResponse: SubHeader Error", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                // Check Network No.
                if (response[2] != 0x00)
                {
                    // Network No Error
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: Network No Error");
                    logger.Create("ParseReadMultiSignedWordsResponse: Network No Error", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                // Parse the data based on your specific protocol

                for (int i = 0; i < length; i++)
                {
                    // Adjust the parsing logic based on your data format
                    defaults[i] = BitConverter.ToInt16(response, 11 + i * 2);
                }
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: " + ex.Message);
                logger.Create("ParseReadMultiSignedWordsResponse: " + ex.Message, LogLevel.Error);
            }
            return defaults;
        }
        private int[] ParseReadMultiSignedDWordsResponse(byte[] response, ushort count)
        {
            try
            {
                if (response == null || response.Length < 5)
                {
                    // Handle error or insufficient data message
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: Invalid response data");
                    logger.Create("ParseReadMultiSignedDWordsResponse: Invalid response data", LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray();
                }

                // Additional parsing logic based on your specific protocol

                // SubHeader Error
                if (response[0] != 0xd0 || response[1] != 0x00)
                {
                    // SubHeader Error
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: SubHeader Error");
                    logger.Create("ParseReadMultiSignedDWordsResponse: SubHeader Error", LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray();
                }

                // Check Network No.
                if (response[2] != 0x00)
                {
                    // Network No Error
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: Network No Error");
                    logger.Create("ParseReadMultiSignedDWordsResponse: Network No Error", LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray();
                }
                int[] values = new int[count];
                for (int i = 0; i < count; i++)
                {
                    int startIndex = 11 + i * 4;

                    // Đọc giá trị signed 32-bit
                    int signedValue = BitConverter.ToInt32(response, startIndex);

                    values[i] = signedValue;
                }
                return values;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: " + ex.Message);
                logger.Create("ParseReadMultiSignedDWordsResponse: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat(0, count).ToArray();
        }
        private short ParseReadSignedWordResponse(byte[] response)
        {
            if(response==null) return 0;
            if (response[7] != 0x04) return 0;
            short value = (short)((response[11]) | response[12] << 8);
            return value;
        }
        private int ParseReadSignedDWordResponse(byte[] response)
        {
            if (response == null) return 0;
            if (response[7] != 0x06) return 0;
            int value = (response[11]) | (response[12] << 8) | (response[13] << 16) | response[14] << 24;
            return value;
        }
        private bool ParseWriteBitResponse(byte[] response)
        {
            try
            {
                if (response == null || response.Length < 12)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteBitResponse: Invalid response data");
                    logger.Create("ParseWriteBitResponse: Invalid response data", LogLevel.Warning);
                    return false;
                }

                if (response[7] != 0x02)
                {
                    // Mã lỗi không phải là mã lỗi "Ghi bit lỗi"
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return false;
                }

                // Ghi thành công
                return true;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteBitResponse: " + ex.Message);
                logger.Create("ParseWriteBitResponse: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        private bool ParseWriteSignedWordResponse(byte[] response)
        {
            if (response == null) return false;
            if (response[7] == 0x02) return true;
            return false;
        }
        private bool ParseWriteSignedDWordResponse(byte[] response)
        {
            if (response == null) return false;
            if (response[7] == 0x02) return true;
            return false;
        }
        private bool ParseWriteMultiBitsResponse(byte[] response, int bitCount)
        {
            if (response == null) return false;
            if (response[7] == 0x02) return true;
            return false;
        }
        private bool ParseWriteMultiWordsResponse(byte[] response, ushort startAddress)
        {
            if (response == null) return false;
            if (response[7] == 0x02) return true;
            return false;
        }
        private bool ParseWriteMultiDWordsResponse(byte[] response, ushort startAddress, int numDWords)
        {
            if (response == null) return false;
            if (response[7] == 0x02) return true;
            return false;
        }

        private async Task SendMCRequest(byte[] request)
        {
            await this.stream.WriteAsync(request, 0, request.Length);
        }
        private async Task WaitReturnData()
        {
            try
            {
                int counterDelayReceiver = 0;
                await Task.Run(async () => {
                    while (!this.isReceiver)
                    {
                        if (counterDelayReceiver > 400)
                        {
                            break;
                        }
                        await Task.Delay(1);
                        counterDelayReceiver++;
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Create("WaitReturnData: " + ex.Message, LogLevel.Error);
            }
        }
        public override Task Send(string dataSend)
        {
            throw new NotImplementedException();
        }
        private void SetDevice(string dev, byte[] request)
        {
            if (dev == "D")
            {
                request[18] = 0xA8; 
            }
            else if(dev == "M")
            {
                request[18] = 0x90;
            }
            else if(dev == "X")
            {
                request[18] = 0x9C;
            }
            else if(dev == "Y")
            {
                request[18] = 0x9D;
            }
            else if (dev == "L")
            {
                request[18] = 0x92;
            }
        }
        public enum DeviceCode { X, Y, M, D,L }
    }
}
