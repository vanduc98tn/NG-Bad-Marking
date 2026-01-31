using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SettingDevice
    {
        public ModbusCOMSetting modbusComSetting { get; set; }
        public ModbusTCPSetting modbusTCPSetting { get; set; }
        public MCEProtocolBinarySetting MCEProtocolBinarySetting { get; set; }
        public COMSetting ScannerCOMSetting { get; set; }
        public TCPSetting ScannerTCPSetting { get; set; }
        public SettingDevice()
        {
            this.modbusComSetting = new ModbusCOMSetting();
            this.modbusTCPSetting = new ModbusTCPSetting();
            this.MCEProtocolBinarySetting = new MCEProtocolBinarySetting();
            this.ScannerCOMSetting = new COMSetting();
            this.ScannerTCPSetting = new TCPSetting();
        }
    }
}
