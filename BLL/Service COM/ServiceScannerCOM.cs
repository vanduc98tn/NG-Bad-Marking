using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class ServiceScannerCOM
    {
        private LoggerDebug logger = new LoggerDebug("ServiceScannerCOM");
        private ScannerCOM scannerCOM;
        public bool IsConnected=false;
        public ServiceScannerCOM(COMSetting comSetting)
        {
            this.scannerCOM = new ScannerCOM(comSetting);
        }
        public async Task<bool> Open()
        {
           return this.IsConnected = await this.scannerCOM.Open();
        }
        public void Close()
        {
            this.scannerCOM.Stop();
        }
        public async Task<string> ReadQrCode(string Bank = "")
        {
            int numberRead = 3;
            String qrRead = "";
            for (int i = 0; i < numberRead; i++)
            {
                qrRead = await this.scannerCOM.Send(Bank);
                if (string.IsNullOrEmpty(qrRead))
                {
                    qrRead = "";
                    continue;
                }
                else if (qrRead == "ERROR\r" || qrRead == "ERROR" || qrRead == "ERROR\r\n" || qrRead == "ERROR\n")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return qrRead;
        }
    }
}
