using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceAOI
    {
        private LoggerDebug logger = new LoggerDebug("ServiceAOI");
        private AOI aOI;
        public bool IsConnected = false;
        public ServiceAOI(TCPSetting tcpSetting) 
        {
            this.IsConnected = false;
            this.aOI = new AOI(tcpSetting.Ip, tcpSetting.Port);
        }
        public async Task<bool> Open()
        {
            return this.IsConnected = await this.aOI.Open();
        }
        public void Close()
        {
            this.IsConnected = false;
            this.aOI.Close();
        }
        public async Task<string> ReadAOI(string cmd)
        {
            int numberRead = 1;
            String aoiRead = "";
            for (int i = 0; i < numberRead; i++)
            {
                aoiRead = await this.aOI.SendToAOI(cmd);
                if (!string.IsNullOrEmpty(aoiRead))
                {
                    break;
                }
            }
            if (string.IsNullOrEmpty(aoiRead))
            {
                return null;
            }
            return aoiRead;
        }
    }
}
