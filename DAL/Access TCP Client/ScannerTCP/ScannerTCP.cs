using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DAL
{
    public class ScannerTCP : BaseTCPClient
    {
        private LoggerDebug logger = new LoggerDebug("ScannerTCP");
        private SemaphoreSlim modbusSemaphore = new SemaphoreSlim(1, 1);
        public ScannerTCP(string ip,int port):base("Scanner TCP", ip, port)
        {

        }
        public async Task<string> SendToScanner(string bankId)
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                await Send(String.Format("LON{0}\r", bankId));
                this.isReceiver = false;
                await WaitReturnData();
                if (this.isReceiver && !string.IsNullOrEmpty(this.dataReceiver))
                {
                    this.notifyEvenTCPClient.NotifyResultUI(this.name, this.dataReceiver);
                    await Send("LOFF\r");
                    this.isReceiver = false;
                    return this.dataReceiver;
                }
                this.notifyEvenTCPClient.NotifyToUI("['" + this.name + "']-Can not Read");
                await Send("LOFF\r");
                return string.Empty;
            }
            finally
            {
                modbusSemaphore.Release();
            }

        }
        public async override Task Send(string dataSend)
        {
            this.isReceiver = false;
            if (this.client == null || !this.client.Connected)
            {
                this.logger.Create("Send Client is not connected ", LogLevel.Warning);
                this.notifyEvenTCPClient.NotifyToUI("['" + this.name + "']-Scanner Is Disconneted!");
                this.notifyEvenTCPClient.NotifyConnectionChange(this.name,false);
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
                        if (counterDelayReceiver > 1000)
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
