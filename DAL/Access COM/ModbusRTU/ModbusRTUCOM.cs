using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class ModbusRTUCOM:BaseCOM
    {
        private LoggerDebug logger = new LoggerDebug("ModbusRTUCOM");
        private ushort slaveAddress;
        
        public ModbusRTUCOM(ModbusCOMSetting comSetting):base("ModbusRTUCOM",comSetting)
        {
            this.slaveAddress = comSetting.AddressSlave;
            
        }
        public async Task<bool> ReadBit(ushort startAddress)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("ReadBit Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadBitRequest(startAddress);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseReadBitResponse(this.txBufReceiver);
            }
            catch(Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("ReadBit: " + ex.Message);
                logger.Create("ReadBit: " + ex.Message, LogLevel.Error);
            }
            
            return false;
        }
        public async Task<List<bool>> ReadMultiBits(ushort startAddress, ushort lengh)
        {
            List<bool> defaults = new List<bool>();
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("ReadMultiBits Unsuccessful, Because Status Is Disconnectd!!!");
                    for (int i = 0; i < lengh; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadMultiBitsRequest(startAddress, lengh);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseReadMultiBitsResponse(this.txBufReceiver, lengh);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("ReadMultiBits: " + ex.Message);
                logger.Create("ReadMultiBits: " + ex.Message, LogLevel.Error);
            }
            for (int i = 0; i < lengh; i++)
            {
                defaults.Add(false);
            }
            return defaults;
        }
        public async Task<short> ReadSignedWord(ushort startAddress)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("ReadSignedWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return 0;
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadWordRequest(startAddress);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseReadSignedWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("ReadSignedWord: " + ex.Message);
                logger.Create("ReadSignedWord: " + ex.Message, LogLevel.Error);
            }
            return 0;
        }
        public async Task<short[]> ReadMultiSignedWords(ushort startAddress, ushort length)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("ReadMultiSignedWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return Enumerable.Repeat((short)0, length).ToArray();
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadMultiWordsRequest(startAddress, length);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseReadMultiSignedWordsResponse(this.txBufReceiver, length);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("ReadMultiSignedWords: " + ex.Message);
                logger.Create("ReadMultiSignedWords: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat((short)0, length).ToArray();
        }
        public async Task<int> ReadSignedDWord(ushort startAddress)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("ReadSignedDWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return 0;
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadDWordRequest(startAddress);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseReadSignedDWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("ReadSignedDWord: " + ex.Message);
                logger.Create("ReadSignedDWord: " + ex.Message, LogLevel.Error);
            }
            return 0;
        }
        public async Task<int[]> ReadMultiSignedDWords(ushort startAddress, ushort count)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("ReadMultiSignedDWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return Enumerable.Repeat(0, count).ToArray();
                }
                this.isReceiver = false;
                byte[] request = this.BuildReadMultiSignedDWordsRequest(startAddress, count);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseReadMultiSignedDWordsResponse(this.txBufReceiver, count);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("ReadMultiSignedDWords: " + ex.Message);
                logger.Create("ReadMultiSignedDWords: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat(0, count).ToArray();
        }
        public async Task<bool> WriteBit(ushort address, bool value)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("WriteBit Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteBitRequest(address, value);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseWriteBitResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("WriteBit: " + ex.Message);
                logger.Create("WriteBit: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> WriteSignedWord(ushort address, short value)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("WriteSignedWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteSignedWordRequest(address, value);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseWriteSignedWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("WriteSignedWord: " + ex.Message);
                logger.Create("WriteSignedWord: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> WriteSignedDWord(ushort address, int value)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("WriteSignedDWord Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteSignedDWordRequest(address, value);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseWriteSignedDWordResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("WriteSignedDWord: " + ex.Message);
                logger.Create("WriteSignedDWord: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> WriteMultiBits(ushort addresses, bool[] values)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("WriteMultiBits Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteMultiBitsRequest(addresses, values);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseWriteMultiBitsResponse(this.txBufReceiver);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("WriteMultiBits: " + ex.Message);
                logger.Create("WriteMultiBits: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> WriteMultiWords(ushort addresses, short[] values)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("WriteMultiWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteMultiWordsRequest(addresses, values);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseWriteMultiWordsResponse(this.txBufReceiver, addresses);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("WriteMultiWords: " + ex.Message);
                logger.Create("WriteMultiWords: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> WriteMultiDWords(ushort addresses, int[] values)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                    this.notifyEvenCOM.NotifyToUI("WriteMultiDWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return false;
                }
                this.isReceiver = false;
                byte[] request = this.BuildWriteMultiDWordsRequest(addresses, values);
                await this.SendModbusRequest(request);
                await WaitReturnData();
                return this.ParseWriteMultiDWordsResponse(this.txBufReceiver, addresses);
            }
            catch (Exception ex)
            {
                this.notifyEvenCOM.NotifyToUI("WriteMultiDWords: " + ex.Message);
                logger.Create("WriteMultiDWords: " + ex.Message, LogLevel.Error);
            }
            return false;
        }


        private async Task SendModbusRequest(byte[] request)
        {
            try
            {
                await this.Send(request);
            }
            catch(Exception ex)
            {
                logger.Create("SendModbusRequest: " + ex.Message,LogLevel.Error);
            }
        }
        public async override Task Send(byte[] txBuf)
        {
            try
            {
                serialPort.DiscardInBuffer();
                await serialPort.BaseStream.WriteAsync(txBuf, 0, txBuf.Length);
            }
            catch (Exception ex)
            {
                logger.Create("Send: " + ex.Message, LogLevel.Error);
            }
        }
        private byte[] BuildReadBitRequest(ushort startAddress)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.ReadCoils);
            // Đặt số lượng bit cần đọc (2 byte)
            request[4] = 0x00;
            request[5] = 0x01;
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildReadMultiBitsRequest(ushort startAddress, ushort length)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.ReadCoils);
            // Đặt số lượng bit cần đọc (2 byte)
            request[4] = (byte)(length >> 8); // Byte cao
            request[5] = (byte)(length & 0xFF); // Byte thấp
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildReadWordRequest(ushort startAddress)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.ReadHoldingRegisters);
            request[4] = 0x00;
            request[5] = 0x01;
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildReadMultiWordsRequest(ushort startAddress, ushort length)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.ReadHoldingRegisters);
            request[4] = (byte)(length >> 8); // Byte cao
            request[5] = (byte)(length & 0xFF); // Byte thấp
                                                // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildReadDWordRequest(ushort startAddress)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.ReadHoldingRegisters);
            request[4] = 0x00;
            request[5] = 0x02; // Đọc 2 từ (32-bit)
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildReadMultiSignedDWordsRequest(ushort startAddress, ushort count)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.ReadHoldingRegisters);
            request[4] = 0x00;
            request[5] = (byte)(count * 2); // Đọc 2 word (4 byte) mỗi word
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildWriteBitRequest(ushort address, bool value)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, address, FuntionModbus.WriteSingleCoil);
            // Đặt giá trị bit cần ghi (2 byte)
            request[4] = value ? (byte)0xFF : (byte)0x00; // Byte thấp
            request[5] = 0x00; // Byte cao
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildWriteSignedWordRequest(ushort address, short value)
        {
            byte[] request = new byte[8];
            // Đặt địa chỉ Slave
            BuildCommon(request, address, FuntionModbus.WriteSingleRegister);
            // Đặt giá trị từ (word) cần ghi (2 byte)
            request[4] = (byte)(value >> 8); // Byte cao
            request[5] = (byte)(value & 0xFF); // Byte thấp
                                               // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 6);
            request[6] = (byte)(crc & 0xFF); // Byte thấp
            request[7] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildWriteSignedDWordRequest(ushort address, int value)
        {
            byte[] request = new byte[13];
            // Đặt địa chỉ Slave
            BuildCommon(request, address, FuntionModbus.WriteMultipleRegisters);
            // Đặt số lượng từ cần ghi (2 byte)
            request[4] = 0x00;
            request[5] = 0x02; // Ghi 2 từ (4 byte)
            // Đặt số lượng byte của dữ liệu (1 byte)
            request[6] = 0x04;
            // Đặt giá trị từ (DWord) cần ghi (4 byte)
            request[9] = (byte)(value >> 24); // Byte cao cao
            request[10] = (byte)(value >> 16); // Byte cao
            request[7] = (byte)(value >> 8);  // Byte thấp
            request[8] = (byte)(value & 0xFF);  // Byte thấp thấp
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 11);
            request[11] = (byte)(crc & 0xFF); // Byte thấp
            request[12] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildWriteMultiBitsRequest(ushort startAddress, bool[] values)
        {
            int numBits = values.Length;
            int numBytes = (numBits + 7) / 8;
            byte[] request = new byte[9 + numBytes];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.WriteMultipleCoils);
            // Đặt số lượng bit cần ghi (2 byte)
            request[4] = (byte)(numBits >> 8); // Byte cao
            request[5] = (byte)(numBits & 0xFF); // Byte thấp
                                                 // Đặt số lượng byte của dữ liệu (1 byte)
            request[6] = (byte)numBytes;
            // Đặt giá trị bit cần ghi (n byte)
            for (int i = 0; i < numBytes; i++)
            {
                request[7 + i] = 0x00;
                for (int j = 0; j < 8; j++)
                {
                    int bitIndex = i * 8 + j;
                    if (bitIndex < numBits && values[bitIndex])
                    {
                        int offset = bitIndex % 8;
                        request[7 + i] |= (byte)(1 << offset);
                    }
                }
            }
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 7 + numBytes);
            request[7 + numBytes] = (byte)(crc & 0xFF); // Byte thấp
            request[8 + numBytes] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildWriteMultiWordsRequest(ushort startAddress, short[] values)
        {
            int numWords = values.Length;
            int numBytes = numWords * 2;
            byte[] request = new byte[9 + numBytes];
            // Đặt địa chỉ Slave
            BuildCommon(request, startAddress, FuntionModbus.WriteMultipleRegisters);
            // Đặt số lượng từ cần ghi (2 byte)
            request[4] = (byte)(numWords >> 8); // Byte cao
            request[5] = (byte)(numWords & 0xFF); // Byte thấp
                                                  // Đặt số lượng byte của dữ liệu (1 byte)
            request[6] = (byte)numBytes;
            // Đặt giá trị từ (word) cần ghi (2n byte)
            for (int i = 0; i < numWords; i++)
            {
                request[7 + i * 2] = (byte)(values[i] >> 8); // Byte cao
                request[8 + i * 2] = (byte)(values[i] & 0xFF); // Byte thấp
            }
            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 7 + numBytes);
            request[7 + numBytes] = (byte)(crc & 0xFF); // Byte thấp
            request[8 + numBytes] = (byte)(crc >> 8);   // Byte cao
            return request;
        }
        private byte[] BuildWriteMultiDWordsRequest(ushort startAddress, int[] values)
        {
            int numDWords = values.Length * 2;
            int numBytes = numDWords * 2;
            byte[] request = new byte[13 + numBytes];

            BuildCommon(request, startAddress, FuntionModbus.WriteMultipleRegisters);

            // Đặt số lượng từ cần ghi (2 byte)
            request[4] = (byte)(numDWords >> 8); // Byte cao
            request[5] = (byte)(numDWords & 0xFF); // Byte thấp

            // Đặt số lượng byte của dữ liệu (2 byte)
            request[6] = (byte)numBytes;

            // Đặt số lượng byte cần ghi cho từng từ (2 byte)

            // Đặt giá trị từ (DWord) cần ghi (4n byte)
            for (int i = 0; i < values.Length; i++)
            {
                request[9 + i * 4] = (byte)(values[i] >> 24); // Byte cao cao
                request[10 + i * 4] = (byte)(values[i] >> 16); // Byte cao
                request[7 + i * 4] = (byte)(values[i] >> 8);  // Byte thấp
                request[8 + i * 4] = (byte)(values[i] & 0xFF);  // Byte thấp thấp
            }

            // Tính toán mã kiểm tra CRC (Checksum)
            ushort crc = CalculateCRC(request, 7 + numBytes);
            request[7 + numBytes] = (byte)(crc & 0xFF); // Byte thấp
            request[8 + numBytes] = (byte)(crc >> 8);   // Byte cao

            return request;
        }

        private void BuildCommon(byte[] request, ushort startAddress, byte funtion)
        {
            request[0] = (byte)slaveAddress;
            request[1] = funtion;
            request[2] = (byte)(startAddress >> 8); // Byte cao
            request[3] = (byte)(startAddress & 0xFF); // Byte thấp
        }


        private bool ParseReadBitResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseReadBitResponse Input response = null: ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadBitResponse Input response = null");
                return false;
            } 
            try
            {
                // Phân tích phản hồi để lấy giá trị bit
                if (response.Length >= 4)
                {
                    // Kiểm tra mã lỗi Modbus RTU
                    if (response[1] == 0x01)
                    {
                        // Lấy giá trị bit từ byte thứ 3 của phản hồi
                        return (response[3] & 0x01) == 0x01;
                    }
                }
                // Xử lý lỗi
            }
            catch (Exception ex)
            {
                logger.Create("ParseReadBitResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadBitResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "ReadBit No Response Data");
            logger.Create("ReadBit No Response Data", LogLevel.Warning);
            return false;
        }
        private List<bool> ParseReadMultiBitsResponse(byte[] response, ushort length)
        {
            var defaults = new List<bool>();
            if (response == null)
            {
                logger.Create("ParseReadMultiBitsResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadMultiBitsResponse Input response = null ");
                for (int i = 0; i < length; i++)
                {
                    defaults.Add(false);
                }
                return defaults;
            }
            List<bool> bitsList = new List<bool>();
            try
            {
                if (response.Length >= 3)
                {
                    // Kiểm tra mã lỗi Modbus RTU
                    if (response[1] == 0x01 && response[2] == (byte)(length / 8 + (length % 8 != 0 ? 1 : 0)))
                    {
                        int byteCount = response[2];
                        byte[] bits = new byte[byteCount];
                        Array.Copy(response, 3, bits, 0, byteCount);

                        // Chuyển đổi mảng byte thành danh sách bit
                        foreach (byte b in bits)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                if (bitsList.Count < length)
                                {
                                    bool bitValue = (b & (1 << i)) != 0;
                                    bitsList.Add(bitValue);
                                }
                            }
                        }
                        // Trả về danh sách bit
                        return bitsList;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseReadMultiBitsResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "ReadMultiBits No Response Data");
            logger.Create("ReadMultiBits No Response Data", LogLevel.Warning);
            for (int i = 0; i < length; i++)
            {
                defaults.Add(false);
            }
            return defaults;
        }
        private short ParseReadSignedWordResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseReadSignedWordResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadSignedWordResponse Input response = null ");
                return 0;
            }
            try
            {
                // Phân tích phản hồi để lấy giá trị từ (word)
                if (response.Length >= 5)
                {
                    // Kiểm tra mã lỗi Modbus RTU
                    if (response[1] == 0x03 && response[2] == 0x02)
                    {
                        // Lấy giá trị từ (word) từ byte thứ 3 và 4 của phản hồi
                        short wordValue = (short)((response[3] << 8) | response[4]);
                        return wordValue;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseReadSignedWordResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadSignedWordResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "ReadSignedWord No Response Data");
            logger.Create("ReadSignedWord No Response Data", LogLevel.Warning);
            return 0;
        }
        private short[] ParseReadMultiSignedWordsResponse(byte[] response, ushort length)
        {
            if (response == null)
            {
                logger.Create("ParseReadMultiSignedWordsResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse Input response = null ");
                return Enumerable.Repeat((short)0, length).ToArray();
            }
            try
            {
                if (response.Length >= 3)
                {
                    // Kiểm tra mã lỗi Modbus RTU
                    if (response[1] == 0x03 && response[2] == (byte)(length * 2))
                    {
                        short[] words = new short[length];
                        for (int i = 0; i < length; i++)
                        {
                            // Lấy giá trị từ (word) từ phản hồi và lưu vào mảng
                            words[i] = (short)((response[3 + i * 2] << 8) | response[4 + i * 2]);
                        }
                        return words;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseReadMultiSignedWordsResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "ReadMultiSignedWords No Response Data");
            logger.Create("ReadMultiSignedWords No Response Data", LogLevel.Warning);
            return Enumerable.Repeat((short)0, length).ToArray(); ;
        }
        private int ParseReadSignedDWordResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseReadSignedDWordResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadSignedDWordResponse Input response = null ");
                return 0;
            }
            try
            {
                if (response.Length >= 7)
                {
                    // Kiểm tra mã lỗi Modbus RTU
                    if (response[1] == 0x03 && response[2] == 0x04)
                    {
                        // Lấy giá trị từ (DWord) từ phản hồi và lưu vào một số nguyên 32-bit
                        int dwordValue = (response[5] << 24) | (response[6] << 16) | (response[3] << 8) | response[4];
                        return dwordValue;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseReadSignedDWordResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadSignedDWordResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "ReadSignedDWord No Response Data");
            logger.Create("ReadSignedDWord No Response Data", LogLevel.Warning);
            return 0;
        }
        private int[] ParseReadMultiSignedDWordsResponse(byte[] response, ushort count)
        {
            if (response == null)
            {
                logger.Create("ParseReadMultiSignedDWordsResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse Input response = null ");
                return Enumerable.Repeat((int)0, count).ToArray();
            }
            List<int> dwordsList = new List<int>();
            try
            {
                // Phân tích phản hồi để lấy giá trị từ (DWord)
                if (response.Length >= 3)
                {
                    // Kiểm tra mã lỗi Modbus RTU
                    if (response[1] == 0x03 && response[2] == (byte)(count * 4))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            // Lấy giá trị từ (DWord) từ phản hồi và chuyển thành số nguyên
                            int dwordValue1 = (response[5 + i * 8] << 24) | (response[6 + i * 8] << 16) | (response[3 + i * 8] << 8) | response[4 + i * 8];
                            int dwordValue2 = (response[9 + i * 8] << 24) | (response[10 + i * 8] << 16) | (response[7 + i * 8] << 8) | response[8 + i * 8];

                            dwordsList.Add(dwordValue1);
                            dwordsList.Add(dwordValue2);
                        }
                        return dwordsList.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseReadMultiSignedDWordsResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "ReadMultiSignedDWords No Response Data");
            logger.Create("ReadMultiSignedDWords No Response Data", LogLevel.Warning);
            return Enumerable.Repeat((int)0, count).ToArray();
        }
        private bool ParseWriteBitResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseWriteBitResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteBitResponse Input response = null ");
                return false;
            }
            try
            {
                if (response.Length >= 4)
                {
                    // Kiểm tra mã lỗi Modbus RTU và function code
                    if (response[1] == 0x05)
                    {
                        // Kiểm tra giá trị của byte 2 để xác nhận ghi bit thành công
                        // Trong trường hợp này, byte 2 cần bằng 0xFF và byte 3 cần bằng 0x00
                        if (response[4] == 0xFF)
                        {
                            // Ghi bit thành công
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteBitResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteBitResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "WriteBit No Response Data");
            logger.Create("WriteBit No Response Data", LogLevel.Warning);
            return false;
        }
        private bool ParseWriteSignedWordResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseWriteSignedWordResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteSignedWordResponse Input response = null ");
                return false;
            }
            try
            {
                if (response.Length >= 4)
                {
                    // Kiểm tra mã lỗi Modbus RTU và function code
                    if (response[1] == 0x06)
                    {
                        // Tính toán và kiểm tra CRC
                        ushort crc = CalculateCRC(response, response.Length - 2);
                        byte crcLowByte = (byte)(crc & 0xFF);
                        byte crcHighByte = (byte)(crc >> 8);

                        if (crcLowByte == response[response.Length - 2] && crcHighByte == response[response.Length - 1])
                        {
                            // CRC hợp lệ và ghi từ (Word) thành công
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteSignedWordResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteSignedWordResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "WriteSignedWord No Response Data");
            logger.Create("WriteSignedWord No Response Data", LogLevel.Warning);
            return false;
        }
        private bool ParseWriteSignedDWordResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseWriteSignedDWordResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteSignedDWordResponse Input response = null ");
                return false;
            }
            try
            {
                if (response.Length >= 4)
                {
                    // Kiểm tra mã lỗi Modbus RTU và function code
                    if (response[1] == 0x10)
                    {
                        // Tính toán và kiểm tra CRC
                        ushort crc = CalculateCRC(response, response.Length - 2);
                        byte crcLowByte = (byte)(crc & 0xFF);
                        byte crcHighByte = (byte)(crc >> 8);

                        if (crcLowByte == response[response.Length - 2] && crcHighByte == response[response.Length - 1])
                        {
                            // CRC hợp lệ và ghi từ (DWord) thành công
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteSignedDWordResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteSignedDWordResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "WriteSignedDWord No Response Data");
            logger.Create("WriteSignedDWord No Response Data", LogLevel.Warning);
            return false;
        }
        private bool ParseWriteMultiBitsResponse(byte[] response)
        {
            if (response == null)
            {
                logger.Create("ParseWriteMultiBitsResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteMultiBitsResponse Input response = null ");
                return false;
            }
            try
            {
                if (response.Length >= 4)
                {
                    // Kiểm tra mã lỗi Modbus RTU và function code
                    if (response[1] == FuntionModbus.WriteMultipleCoils)
                    {
                        // Tính toán và kiểm tra CRC
                        ushort crc = CalculateCRC(response, response.Length - 2);
                        byte crcLowByte = (byte)(crc & 0xFF);
                        byte crcHighByte = (byte)(crc >> 8);

                        if (crcLowByte == response[response.Length - 2] && crcHighByte == response[response.Length - 1])
                        {
                            // CRC hợp lệ và ghi nhiều bit thành công
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteMultiBitsResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteMultiBitsResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "WriteMultiBits No Response Data");
            logger.Create("WriteMultiBits No Response Data", LogLevel.Warning);
            return false;
        }
        private bool ParseWriteMultiWordsResponse(byte[] response, ushort addresses)
        {
            if (response == null)
            {
                logger.Create("ParseWriteMultiWordsResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteMultiWordsResponse Input response = null ");
                return false;
            }
            try
            {
                if (response.Length >= 6)
                {
                    // Kiểm tra mã lỗi Modbus RTU và function code
                    if (response[1] == 0x10)
                    {
                        if (response[2] == (byte)(addresses >> 8) && response[3] == (byte)(addresses & 0xFF))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteMultiWordsResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteMultiWordsResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "WriteMultiWords No Response Data");
            logger.Create("WriteMultiWords No Response Data", LogLevel.Warning);
            return false;
        }
        private bool ParseWriteMultiDWordsResponse(byte[] response, ushort addresses)
        {
            if (response == null)
            {
                logger.Create("ParseWriteMultiDWordsResponse Input response = null ", LogLevel.Warning);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteMultiDWordsResponse Input response = null ");
                return false;
            }
            try
            {
                if (response.Length >= 6)
                {
                    // Kiểm tra mã lỗi Modbus RTU và function code
                    if (response[1] == 0x10)
                    {
                        if (response[2] == (byte)(addresses >> 8) && response[3] == (byte)(addresses & 0xFF))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteMultiDWordsResponse: " + ex.Message, LogLevel.Error);
                this.notifyEvenCOM.NotifyResultUI(this.name, "ParseWriteMultiDWordsResponse: " + ex.Message);
            }
            this.notifyEvenCOM.NotifyResultUI(this.name, "WriteMultiDWords No Response Data");
            logger.Create("WriteMultiDWords No Response Data", LogLevel.Warning);
            return false;
        }
        private ushort CalculateCRC(byte[] data, int length)
        {
            ushort crc = 0xFFFF;
            for (int i = 0; i < length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }
            return crc;
        }
        private async Task WaitReturnData()
        {
            try
            {
                int counterDelayReceiver = 0;
                await Task.Run(async () => {
                    while (!this.isReceiver)
                    {
                        if (counterDelayReceiver > 4)
                        {
                            break;
                        }
                        await Task.Delay(2);
                        counterDelayReceiver++;
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Create("WaitReturnData: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
