using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class ServiceScannerTCP
    {
        private LoggerDebug logger = new LoggerDebug("ServiceScannerTCP");
        private ScannerTCP scannerTCP;
        public bool IsConnected = false;
        public ServiceScannerTCP(TCPSetting tcpSetting)
        {
            this.scannerTCP = new ScannerTCP(tcpSetting.Ip, tcpSetting.Port);
        }
        public async Task<bool> Open()
        {
            return IsConnected = await this.scannerTCP.Open();
        }
        public void Close()
        {
            this.scannerTCP.Close();
            IsConnected = false;
        }
        public async Task<string> ReadQrCode(string bankId="")
        {
            int numberRead = 3;
            String qrRead = "";
            for (int i = 0;i< numberRead; i++)
            {
                qrRead = await this.scannerTCP.SendToScanner(bankId);
                if(string.IsNullOrEmpty(qrRead))
                {
                    continue;
                }
                else if (qrRead == "ERROR\r" || qrRead == "ERROR" || qrRead =="ERROR\r\n")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            qrRead = qrRead.Replace("\r\n", "").Replace("\u0002", "").Replace("\u0018", "");
            return qrRead;
        }
    }
}
