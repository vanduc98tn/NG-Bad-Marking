using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class ModbusTCP : BaseTCPClient
    {
        private LoggerDebug logger = new LoggerDebug("ModbusTCP");
        private ushort slaveAddress;
        public ModbusTCP(ushort slaveAddress, string ip, int port) : base("Modbus TCP", ip, port)
        {
            this.slaveAddress = slaveAddress;
        }
        public async Task<bool> ReadBit(ushort startAddress)
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
                byte[] request = this.BuildReadBitRequest(startAddress);
                await this.SendModbusRequest(request);
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
        public async Task<List<bool>> ReadMultiBits(ushort startAddress, ushort quantity)
        {
            List<bool> defaults = new List<bool>();
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyToUI("ReadMultiBits Unsuccessful, Because Status Is Disconnectd!!!");
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
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
                byte[] request = this.BuildReadMultiBitsRequest(startAddress, quantity);
                await this.SendModbusRequest(request);
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
        public async Task<short> ReadSignedWord(ushort startAddress)
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
                byte[] request = BuildReadSignedWordRequest(startAddress);
                await SendModbusRequest(request);
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
        public async Task<short[]> ReadMultiSignedWords(ushort startAddress, ushort length)
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
                byte[] request = BuildReadMultiSignedWordsRequest(startAddress, length);
                await SendModbusRequest(request);
                await WaitReturnData();
                return ParseReadMultiSignedWordsResponse(this.txBufReceiver, length);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedWords: " + ex.Message);
                logger.Create("ReadMultiSignedWords: " + ex.Message, LogLevel.Error);
            }

            // Trả về một mảng có độ dài chứa các giá trị mặc định (0 trong trường hợp này)
            return Enumerable.Repeat((short)0, length).ToArray();
        }
        public async Task<int> ReadSignedDWord(ushort startAddress)
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
                byte[] request = BuildReadSignedDWordRequest(startAddress);
                await SendModbusRequest(request);
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
        public async Task<int[]> ReadMultiSignedDWords(ushort startAddress, ushort count)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedDWords Unsuccessful, Because Status Is Disconnectd!!!");
                    return Enumerable.Repeat(0, count).ToArray();
                }
                else
                {
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                }
                this.isReceiver = false;
                byte[] request = BuildReadMultiSignedDWordsRequest(startAddress, count);
                await SendModbusRequest(request);
                await WaitReturnData();
                return ParseReadMultiSignedDWordsResponse(this.txBufReceiver, count);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("ReadMultiSignedDWords: " + ex.Message);
                logger.Create("ReadMultiSignedDWords: " + ex.Message, LogLevel.Error);
            }

            return Enumerable.Repeat(0, count).ToArray(); // Hoặc giá trị mặc định khác nếu có lỗi
        }
        public async Task<bool> WriteBit(ushort address, bool value)
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
                byte[] request = BuildWriteBitRequest(address, value);
                await SendModbusRequest(request);
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
        public async Task<bool> WriteSignedWord(ushort address, short value)
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
                byte[] request = BuildWriteSignedWordRequest(address, value);
                await SendModbusRequest(request);
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
        public async Task<bool> WriteSignedDWord(ushort address, int value)
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
                byte[] request = BuildWriteSignedDWordRequest(address, value);
                await SendModbusRequest(request);
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
        public async Task<bool> WriteMultiBits(ushort address, bool[] values)
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
                byte[] request = BuildWriteMultiBitsRequest(address, values);
                await SendModbusRequest(request);
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
        public async Task<bool> WriteMultiWords(ushort addresses, short[] values)
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
                byte[] request = this.BuildWriteMultiWordsRequest(addresses, values);
                await this.SendModbusRequest(request);
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
        public async Task<bool> WriteMultiDWords(ushort addresses, int[] values)
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
                byte[] request = this.BuildWriteMultiDWordsRequest(addresses, values);
                await this.SendModbusRequest(request);
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





        private async Task SendModbusRequest(byte[] request)
        {
            try
            {
                await this.stream.WriteAsync(request, 0, request.Length);
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
            }
        }
        public override Task Send(string dataSend)
        {
            throw new NotImplementedException();
        }

        private byte[] BuildReadBitRequest(ushort startAddress)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.ReadCoils, startAddress);
            // Số lượng (2 bytes)
            request[10] = 0x00;
            request[11] = 0x01;
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildReadMultiBitsRequest(ushort startAddress, ushort quantity)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.ReadCoils, startAddress);

            // Số lượng (2 bytes)
            request[10] = (byte)(quantity >> 8);
            request[11] = (byte)(quantity & 0xFF);
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildReadSignedWordRequest(ushort startAddress)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.ReadHoldingRegisters, startAddress);

            // Số lượng (2 bytes)
            request[10] = 0x00;
            request[11] = 0x01;
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildReadMultiSignedWordsRequest(ushort startAddress, ushort length)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.ReadHoldingRegisters, startAddress);

            // Số lượng (2 bytes)
            request[10] = (byte)(length >> 8);
            request[11] = (byte)(length & 0xFF);
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildReadSignedDWordRequest(ushort startAddress)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.ReadHoldingRegisters, startAddress);

            // Số lượng (2 bytes)
            request[10] = 0x00;
            request[11] = 0x02; // Đọc 2 thanh ghi cho một DWORD
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildReadMultiSignedDWordsRequest(ushort startAddress, ushort count)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.ReadHoldingRegisters, startAddress);

            // Số lượng (2 bytes)
            request[10] = (byte)((count * 2) >> 8);
            request[11] = (byte)((count * 2) & 0xFF);
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildWriteBitRequest(ushort address, bool value)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.WriteSingleCoil, address);

            // Dữ liệu ghi
            request[10] = (byte)(value ? 0xFF : 0x00);
            request[11] = 0x00;
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildWriteSignedWordRequest(ushort address, short value)
        {
            byte[] request = new byte[12]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.WriteSingleRegister, address);

            // Dữ liệu ghi (16-bit)
            request[10] = (byte)(value >> 8);
            request[11] = (byte)(value & 0xff);
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildWriteSignedDWordRequest(ushort address, int value)
        {
            byte[] request = new byte[14]; // Độ dài của bản tin Modbus TCP
            BuildCommon(request, FuntionModbus.WriteSingleRegister, address);
            // Dữ liệu ghi (32-bit)
            request[11] = (byte)(value & 0xFF);
            request[10] = (byte)((value >> 8) & 0xFF);
            request[13] = (byte)((value >> 16) & 0xFF);
            request[12] = (byte)((value >> 24) & 0xFF);
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildWriteMultiBitsRequest(ushort startAddress, bool[] values)
        {
            int numBits = values.Length;
            int numBytes = (numBits + 7) / 8;
            byte[] request = new byte[15 + numBytes - 2]; // Thêm 12 byte cho tiêu đề Modbus TCP
            BuildCommon(request, FuntionModbus.WriteMultipleCoils, startAddress);
            // Đặt số lượng coils cần ghi (2 byte)
            request[10] = (byte)(numBits >> 8); // Byte cao
            request[11] = (byte)(numBits & 0xFF); // Byte thấp
            // Đặt số lượng byte dữ liệu
            request[12] = (byte)numBytes;
            // Đặt giá trị bit cần ghi (n byte)
            for (int i = 0; i < numBytes; i++)
            {
                request[13 + i] = 0x00;
                for (int j = 0; j < 8; j++)
                {
                    int bitIndex = i * 8 + j;
                    if (bitIndex < numBits && values[bitIndex])
                    {
                        int offset = bitIndex % 8;
                        request[13 + i] |= (byte)(1 << offset);
                    }
                }
            }
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildWriteMultiWordsRequest(ushort startAddress, short[] values)
        {
            int numWords = values.Length;
            int numBytes = numWords * 2; // Mỗi từ là 2 byte
            byte[] request = new byte[15 + numBytes]; // Thêm 12 byte cho tiêu đề Modbus TCP
            BuildCommon(request, FuntionModbus.WriteMultipleRegisters, startAddress);
            // Đặt số lượng words cần ghi (2 byte)
            request[10] = (byte)(numWords >> 8); // Byte cao
            request[11] = (byte)(numWords & 0xFF); // Byte thấp
                                                   // Đặt số lượng byte dữ liệu
            request[12] = (byte)numBytes;
            // Đặt giá trị từ (word) cần ghi (2n byte)
            for (int i = 0; i < numWords; i++)
            {
                request[13 + i * 2] = (byte)(values[i] >> 8); // Byte cao
                request[14 + i * 2] = (byte)(values[i] & 0xFF); // Byte thấp
            }
            request[5] = (byte)(request.Length - 6);
            return request;
        }
        private byte[] BuildWriteMultiDWordsRequest(ushort startAddress, int[] values)
        {
            int numDWords = values.Length * 2;
            int numBytes = numDWords * 2;
            byte[] request = new byte[13 + numBytes]; // Thêm 12 byte cho tiêu đề Modbus TCP
            BuildCommon(request, FuntionModbus.WriteMultipleRegisters, startAddress);
            // Đặt số lượng DWords cần ghi (2 byte)
            request[10] = (byte)(numDWords >> 8); // Byte cao
            request[11] = (byte)(numDWords & 0xFF); // Byte thấp
            // Đặt số lượng byte dữ liệu
            request[12] = (byte)(numBytes);
            // Đặt giá trị DWord cần ghi (4n byte)
            for (int i = 0; i < values.Length; i++)
            {
                byte byte1 = (byte)(values[i] >> 24);
                byte byte2 = (byte)(values[i] >> 16);
                byte byte3 = (byte)(values[i] >> 8);
                byte byte4 = (byte)(values[i] & 0xFF);
                request[15 + i * 4] = byte1;
                request[16 + i * 4] = byte2;
                request[13 + i * 4] = byte3;
                request[14 + i * 4] = byte4;
            }
            request[5] = (byte)(request.Length - 6);
            return request;
        }

        private void BuildCommon(byte[] request, byte functionCode, ushort startAddress)
        {
            // Transaction ID (2 bytes) - Đây có thể là một số ngẫu nhiên
            request[0] = 0x00;
            request[1] = 0x01;

            // Protocol ID (2 bytes) - Modbus TCP có giá trị là 0x0000
            request[2] = 0x00;
            request[3] = 0x00;

            // Độ dài (2 bytes) - Sẽ được tính toán sau cùng
            request[4] = 0x00;
            request[5] = 0x06;

            // Unit ID (1 byte) - Địa chỉ của thiết bị Modbus, thường là 0x01
            request[6] = 0x01;

            // Function Code (1 byte) - Tùy thuộc vào tham số functionCode
            request[7] = functionCode;

            // Địa chỉ bắt đầu (2 bytes)
            request[8] = (byte)(startAddress >> 8);
            request[9] = (byte)(startAddress & 0xFF);
        }

        private bool ParseReadBitResponse(byte[] response)
        {
            if (response == null || response.Length < 9)
            {
                // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadBitResponse: Invalid response data");
                logger.Create("ParseReadBitResponse: Invalid response data", LogLevel.Warning);
                return false;
            }
            // Kiểm tra Unit ID (Số hiệu thiết bị)
            if (response[6] != 0x01)
            {
                // Unit ID không đúng
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadBitResponse: Invalid Unit ID");
                logger.Create("ParseReadBitResponse: Invalid Unit ID", LogLevel.Warning);
                return false;
            }
            // Kiểm tra mã lỗi (Exception Code)
            if (response[7] >= 0x80 && response[7] <= 0xFF)
            {
                // Có mã lỗi
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                return false;
            }
            // Kiểm tra độ dài dữ liệu trả về
            int dataLength = response[8];
            if (dataLength != 1)
            {
                // Độ dài dữ liệu không đúng
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadBitResponse: Invalid data length");
                logger.Create("ParseReadBitResponse: Invalid data length", LogLevel.Warning);
                return false;
            }
            //Lấy giá trị bit từ dữ liệu trả về
            byte dataByte = response[9]; // Lấy byte thứ 9 của phản hồi
            int bitIndex = 0; // Index của bit bạn muốn đọc (0 - 7)
            return (dataByte & (1 << bitIndex)) != 0;
        }
        private List<bool> ParseReadMultiBitsResponse(byte[] response, ushort quantity)
        {
            var defaults = new List<bool>();
            try
            {
                if (response == null || response.Length < 9)
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
                // Kiểm tra Unit ID (Số hiệu thiết bị)
                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: Invalid Unit ID");
                    logger.Create("ParseReadMultiBitsResponse: Invalid Unit ID", LogLevel.Warning);
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                // Kiểm tra mã lỗi (Exception Code)
                if (response[7] >= 0x80 && response[7] <= 0xFF)
                {
                    // Có mã lỗi
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                // Kiểm tra độ dài dữ liệu trả về
                int dataLength = response[8];
                if (dataLength != (quantity / 8) + ((quantity % 8) > 0 ? 1 : 0))
                {
                    // Độ dài dữ liệu không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiBitsResponse: Invalid data length");
                    logger.Create("ParseReadMultiBitsResponse: Invalid data length", LogLevel.Warning);
                    for (int i = 0; i < quantity; i++)
                    {
                        defaults.Add(false);
                    }
                    return defaults;
                }
                // Lấy giá trị bit từ dữ liệu trả về
                byte[] dataBytes = new byte[dataLength];
                Array.Copy(response, 9, dataBytes, 0, dataLength);
                // Chuyển dữ liệu byte thành danh sách các giá trị boolean
                List<bool> bitValues = new List<bool>();
                for (int i = 0; i < quantity; i++)
                {
                    int byteIndex = i / 8;
                    int bitIndex = i % 8;
                    bool bitValue = (dataBytes[byteIndex] & (1 << bitIndex)) != 0;
                    bitValues.Add(bitValue);
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
        private short ParseReadSignedWordResponse(byte[] response)
        {
            try
            {
                if (response == null || response.Length < 11)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedWordResponse: Invalid response data");
                    logger.Create("ParseReadSignedWordResponse: Invalid response data", LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Kiểm tra Unit ID (Số hiệu thiết bị)
                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedWordResponse: Invalid Unit ID");
                    logger.Create("ParseReadSignedWordResponse: Invalid Unit ID", LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Kiểm tra mã lỗi (Exception Code)
                if (response[7] >= 0x80 && response[7] <= 0xFF)
                {
                    // Có mã lỗi
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Kiểm tra độ dài dữ liệu trả về
                int dataLength = response[8];
                if (dataLength != 2)
                {
                    // Độ dài dữ liệu không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedWordResponse: Invalid data length");
                    logger.Create("ParseReadSignedWordResponse: Invalid data length", LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Lấy giá trị từ dữ liệu trả về
                short value = (short)((response[9] << 8) | response[10]);
                return value;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedWordResponse: " + ex.Message);
                logger.Create("ParseReadSignedWordResponse: " + ex.Message, LogLevel.Error);
            }
            return 0;
        }
        private short[] ParseReadMultiSignedWordsResponse(byte[] response, ushort length)
        {
            try
            {
                if (response == null || response.Length < 9)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: Dữ liệu phản hồi không hợp lệ");
                    logger.Create("ParseReadMultiSignedWordsResponse: Dữ liệu phản hồi không hợp lệ", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: Unit ID không hợp lệ");
                    logger.Create("ParseReadMultiSignedWordsResponse: Unit ID không hợp lệ", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                if (response[7] >= 0x80 && response[7] <= 0xFF)
                {
                    // Mã lỗi Modbus
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Mã lỗi Modbus: " + response[7]);
                    logger.Create("Mã lỗi Modbus: " + response[7], LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                int dataLength = response[8];
                if (dataLength != (length * 2))
                {
                    // Độ dài dữ liệu không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: Độ dài dữ liệu không hợp lệ");
                    logger.Create("ParseReadMultiSignedWordsResponse: Độ dài dữ liệu không hợp lệ", LogLevel.Warning);
                    return Enumerable.Repeat((short)0, length).ToArray();
                }

                short[] values = new short[length];
                for (int i = 0; i < length; i++)
                {
                    values[i] = (short)((response[9 + i * 2] << 8) | response[10 + i * 2]);
                }
                return values;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedWordsResponse: " + ex.Message);
                logger.Create("ParseReadMultiSignedWordsResponse: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat((short)0, length).ToArray();
        }
        private int ParseReadSignedDWordResponse(byte[] response)
        {
            try
            {
                if (response == null || response.Length < 13)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedDWordResponse: Invalid response data");
                    logger.Create("ParseReadSignedDWordResponse: Invalid response data", LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedDWordResponse: Invalid Unit ID");
                    logger.Create("ParseReadSignedDWordResponse: Invalid Unit ID", LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                if (response[7] >= 0x80 && response[7] <= 0xFF)
                {
                    // Mã lỗi Modbus
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Kiểm tra độ dài dữ liệu trả về
                int dataLength = response[8];
                if (dataLength != 4)
                {
                    // Độ dài dữ liệu không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedDWordResponse: Invalid data length");
                    logger.Create("ParseReadSignedDWordResponse: Invalid data length", LogLevel.Warning);
                    return 0; // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Lấy giá trị từ dữ liệu trả về
                int value = (response[11] << 24) | (response[12] << 16) | (response[9] << 8) | response[10];
                return value;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadSignedDWordResponse: " + ex.Message);
                logger.Create("ParseReadSignedDWordResponse: " + ex.Message, LogLevel.Error);
            }
            return 0; // Hoặc giá trị mặc định khác nếu có lỗi
        }
        private int[] ParseReadMultiSignedDWordsResponse(byte[] response, ushort count)
        {
            try
            {
                if (response == null || response.Length < 9 + count * 4)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: Invalid response data");
                    logger.Create("ParseReadMultiSignedDWordsResponse: Invalid response data", LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray(); // Hoặc giá trị mặc định khác nếu có lỗi
                }

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: Invalid Unit ID");
                    logger.Create("ParseReadMultiSignedDWordsResponse: Invalid Unit ID", LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray(); // Hoặc giá trị mặc định khác nếu có lỗi
                }

                if (response[7] >= 0x80 && response[7] <= 0xFF)
                {
                    // Mã lỗi Modbus
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray(); // Hoặc giá trị mặc định khác nếu có lỗi
                }

                // Kiểm tra độ lẹ dữ liệu trả về
                int dataLength = response[8];
                if (dataLength != count * 4)
                {
                    // Độ dài dữ liệu không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: Invalid data length");
                    logger.Create("ParseReadMultiSignedDWordsResponse: Invalid data length", LogLevel.Warning);
                    return Enumerable.Repeat(0, count).ToArray(); // Hoặc giá trị mặc định khác nếu có lỗi
                }

                int[] values = new int[count];
                for (int i = 0; i < count; i++)
                {
                    int startIndex = 9 + i * 4;
                    values[i] = (response[startIndex + 2] << 24) | (response[startIndex + 3] << 16) | (response[startIndex] << 8) | response[startIndex + 1];
                }
                return values;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseReadMultiSignedDWordsResponse: " + ex.Message);
                logger.Create("ParseReadMultiSignedDWordsResponse: " + ex.Message, LogLevel.Error);
            }
            return Enumerable.Repeat(0, count).ToArray(); // Hoặc giá trị mặc định khác nếu có lỗi
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

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteBitResponse: Invalid Unit ID");
                    logger.Create("ParseWriteBitResponse: Invalid Unit ID", LogLevel.Warning);
                    return false;
                }

                if (response[7] != 0x05)
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
            try
            {
                if (response == null || response.Length < 12)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteSignedWordResponse: Invalid response data");
                    logger.Create("ParseWriteSignedWordResponse: Invalid response data", LogLevel.Warning);
                    return false;
                }

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteSignedWordResponse: Invalid Unit ID");
                    logger.Create("ParseWriteSignedWordResponse: Invalid Unit ID", LogLevel.Warning);
                    return false;
                }

                if (response[7] != 0x06)
                {
                    // Mã lỗi không phải là mã lỗi "Ghi số nguyên lỗi"
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return false;
                }

                // Ghi thành công
                return true;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteSignedWordResponse: " + ex.Message);
                logger.Create("ParseWriteSignedWordResponse: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        private bool ParseWriteSignedDWordResponse(byte[] response)
        {
            try
            {
                if (response == null || response.Length < 12)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteSignedDWordResponse: Invalid response data");
                    logger.Create("ParseWriteSignedDWordResponse: Invalid response data", LogLevel.Warning);
                    return false;
                }

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteSignedDWordResponse: Invalid Unit ID");
                    logger.Create("ParseWriteSignedDWordResponse: Invalid Unit ID", LogLevel.Warning);
                    return false;
                }

                if (response[7] != 0x06)
                {
                    // Mã lỗi không phải là mã lỗi "Ghi số nguyên lỗi"
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return false;
                }

                // Ghi thành công
                return true;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteSignedDWordResponse: " + ex.Message);
                logger.Create("ParseWriteSignedDWordResponse: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        private bool ParseWriteMultiBitsResponse(byte[] response, int bitCount)
        {
            try
            {
                if (response == null || response.Length < 12)
                {
                    // Xử lý lỗi hoặc thông báo không đủ dữ liệu
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteMultiBitsResponse: Invalid response data");
                    logger.Create("ParseWriteMultiBitsResponse: Invalid response data", LogLevel.Warning);
                    return false;
                }

                if (response[6] != 0x01)
                {
                    // Unit ID không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteMultiBitsResponse: Invalid Unit ID");
                    logger.Create("ParseWriteMultiBitsResponse: Invalid Unit ID", LogLevel.Warning);
                    return false;
                }

                if (response[7] != 0x0F)
                {
                    // Mã lỗi không phải là mã lỗi "Ghi nhiều bit lỗi"
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return false;
                }

                if (response[11] != bitCount)
                {
                    // Độ dài dữ liệu không đúng
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteMultiBitsResponse: Invalid data length");
                    logger.Create("ParseWriteMultiBitsResponse: Invalid data length", LogLevel.Warning);
                    return false;
                }

                // Ghi thành công
                return true;
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteMultiBitsResponse: " + ex.Message);
                logger.Create("ParseWriteMultiBitsResponse: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        private bool ParseWriteMultiWordsResponse(byte[] response, ushort startAddress)
        {
            try
            {
                // Kiểm tra độ dài của phản hồi
                if (response.Length < 12)
                {
                    // Phản hồi không hợp lệ
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteMultiWordsResponse: Invalid data length");
                    logger.Create("ParseWriteMultiWordsResponse: Invalid data length", LogLevel.Warning);
                    return false;
                }

                // Kiểm tra Unit Identifier (địa chỉ thiết bị)
                if (response[6] != slaveAddress)
                {
                    // Địa chỉ không khớp với thiết bị Modbus Slave
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "ParseWriteMultiWordsResponse: Invalid Unit ID");
                    logger.Create("ParseWriteMultiWordsResponse: Invalid Unit ID", LogLevel.Warning);
                    return false;
                }

                // Kiểm tra Function Code
                if (response[7] != FuntionModbus.WriteMultipleRegisters)
                {
                    // Không phải phản hồi cho truy vấn ghi nhiều từ
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, "Modbus Exception Code: " + response[7]);
                    logger.Create("Modbus Exception Code: " + response[7], LogLevel.Warning);
                    return false;
                }

                // Kiểm tra địa chỉ bắt đầu
                ushort responseStartAddress = (ushort)((response[8] << 8) | response[9]);
                if (responseStartAddress != startAddress)
                {
                    // Địa chỉ bắt đầu trong phản hồi không khớp với yêu cầu
                    return false;
                }

                // Kiểm tra số lượng words được ghi
                ushort responseWordCount = (ushort)((response[10] << 8) | response[11]);
                if (responseWordCount != startAddress)
                {
                    // Số lượng words không khớp với yêu cầu
                    return false;
                }

                // Kiểm tra mã lỗi
                byte exceptionCode = response[8];
                if (exceptionCode != 0)
                {
                    // Mã lỗi không phải là 0, có lỗi xảy ra
                    return false;
                }

                // Nếu tất cả kiểm tra trước đó đều vượt qua, phản hồi hợp lệ
                return true;
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteMultiWordsResponse: " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        private bool ParseWriteMultiDWordsResponse(byte[] response, ushort startAddress, int numDWords)
        {
            try
            {
                if (response.Length < 9)
                {
                    return false; // Phản hồi quá ngắn
                }

                if (response[7] != FuntionModbus.WriteMultipleRegisters)
                {
                    return false; // Không phải phản hồi cho truy vấn ghi nhiều DWords
                }

                if (response[8] != (startAddress >> 8) || response[9] != (startAddress & 0xFF))
                {
                    return false; // Địa chỉ bắt đầu không khớp
                }

                if (response[10] != (numDWords >> 8) || response[11] != (numDWords & 0xFF))
                {
                    return false; // Số lượng DWords không khớp
                }

                if (response[12] != 0x00 || response[13] != 0x00)
                {
                    return false; // Mã lỗi không hợp lệ
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Create("ParseWriteMultiDWordsResponse: " + ex.Message, LogLevel.Error);
            }
            return false; // Không phải phản hồi cho truy vấn ghi nhiều DWords
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
    }
}
