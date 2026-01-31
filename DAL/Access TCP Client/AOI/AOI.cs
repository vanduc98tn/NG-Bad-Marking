using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class AOI : BaseTCPClient
    {
        private LoggerDebug logger = new LoggerDebug("AOI");
        public AOI(string serverIp, int serverPort) : base("AOI", serverIp, serverPort)
        {
        }
        public async Task<string> SendToAOI(string dataSend)
        {
            await Send(String.Format(dataSend));
            await WaitReturnData();
            if (this.isReceiver && !string.IsNullOrEmpty(this.dataReceiver))
            {
                this.notifyEvenTCPClient.NotifyResultUI(this.name, this.dataReceiver);
                this.isReceiver = false;
                return this.dataReceiver;
            }
            this.notifyEvenTCPClient.NotifyToUI("['" + this.name + "']-Can not Read From AOI");
            return string.Empty;
        }
        public async override Task Send(string dataSend)
        {
            this.isReceiver = false;
            this.dataReceiver = "";
            if (this.client == null || !this.client.Connected)
            {
                this.logger.Create("Send Client is not connected ", LogLevel.Warning);
                this.notifyEvenTCPClient.NotifyToUI("['" + this.name + "']-AOI Is Disconneted!");
                this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                return;
            }
            byte[] buffer = Encoding.UTF8.GetBytes(dataSend);
            await this.stream.WriteAsync(buffer, 0, buffer.Length);
        }
        private async Task WaitReturnData()
        {
            try
            {
                int counterDelayReceiver = 0;
                await Task.Run(async () => {
                    while (!this.isReceiver)
                    {
                        if (counterDelayReceiver > 700)
                        {
                            break;
                        }
                        await Task.Delay(10);
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
