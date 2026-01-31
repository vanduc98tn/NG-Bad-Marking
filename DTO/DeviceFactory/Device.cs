using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public interface Device
    {
        Task<bool> Open();
        void Close();
        Task<bool> ReadBit(ushort startAddress, string DeviceName = "");
        Task<List<bool>> ReadMultiBits(ushort startAddress, ushort quantity, string DeviceName = "");
        Task<short> ReadSignedWord(ushort startAddress, string DeviceName = "");
        Task<short[]> ReadMultiSignedWords(ushort startAddress, ushort length, string DeviceName = "");
        Task<int> ReadSignedDWord(ushort startAddress, string DeviceName = "");
        Task<int[]> ReadMultiSignedDWords(ushort startAddress, ushort count, string DeviceName = "");
        Task<bool> WriteBit(ushort address, bool value, string DeviceName = "");
        Task<bool> WriteSignedWord(ushort address, short value, string DeviceName = "");
        Task<bool> WriteSignedDWord(ushort address, int value, string DeviceName = "");
        Task<bool> WriteMultiBits(ushort address, bool[] values, string DeviceName = "");
        Task<bool> WriteMultiWords(ushort addresses, short[] values, string DeviceName = "");
        Task<bool> WriteMultiDWords(ushort addresses, int[] values, string DeviceName = "");
    }
}
