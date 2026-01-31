using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class FileAlarm
    {
        public string Device { get; set; }
        public int DeviceCode { get; set; }
        public string Block { get; set; }
        public string Message { get; set; }
        public string Solution { get; set; }
    }
}
