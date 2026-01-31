using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ScannerCOMSetting:COMSetting
    {
        public string BankID { get; set; }
        public ScannerCOMSetting()
        {
            this.BankID = "01";
            this.portName = "COM1";
            this.baudrate = 9600;
            this.dataBits = 8;
            this.stopBits = StopBits.One;
            this.parity = Parity.None;
        }
    }
}
