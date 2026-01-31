using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using DTO;

namespace DAL
{
    public class ScannerCOM : BaseCOM
    {
        private LoggerDebug logger = new LoggerDebug("ScannerCOM");
        public ScannerCOM(COMSetting comSetting) : base("ScannerCOM", comSetting)
        {
            this.serialPort.ReadTimeout = 1000;
        }
        public override async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //base.SerialPort_DataReceived(sender, e);
            try
            {
                await Task.Delay(500);
                var receivedData = await Task.Run(() => serialPort.ReadExisting());
                this.notifyEvenCOM.NotifyResultUI(this.name, receivedData);
                if (!string.IsNullOrEmpty(receivedData))
                {
                    this.notifyEvenCOM.NotifyResultUI(this.name, receivedData);
                    this.dataReceiver = receivedData;
                    this.isReceiver = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<string> Send(string bankId)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    this.notifyEvenCOM.NotifyToUI(String.Format("[{0}]-Read Qr Use Bank = " + bankId, this.name));
                    this.isReceiver = false;
                    await this.SendTo(String.Format("LON{0}\r", bankId));
                    await WaitReturnData();
                    if (this.isReceiver && !string.IsNullOrEmpty(this.dataReceiver))
                    {
                        await SendTo("LOFF\r");
                        this.isReceiver = false;
                        return this.dataReceiver;
                    }
                    await this.SendTo("LOFF\r");
                    this.notifyEvenCOM.NotifyToUI(String.Format("[{0}]-Read Qr Is Empty", this.name));
                    return string.Empty;
                }
                this.notifyEvenCOM.CheckConnectChange(this.name, false);
                this.notifyEvenCOM.NotifyToUI(String.Format("[{0}]-Read Qr Is Faild Port Is Closed", this.name));
            }
            catch (Exception ex)
            {
                logger.Create("Send: " + ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }
        public async Task SendTo(string cmd)
        {
            var buffer = ASCIIEncoding.ASCII.GetBytes(cmd);
            await this.Send(buffer);
        }
        public async override Task Send(byte[] txBuf)
        {
            await serialPort.BaseStream.WriteAsync(txBuf, 0, txBuf.Length);
        }
        private async Task WaitReturnData()
        {
            try
            {
                int counterDelayReceiver = 0;
                await Task.Run(async () => {
                    while (!this.isReceiver)
                    {
                        if (counterDelayReceiver > 200)
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
                logger.Create("WaitMESReturnData: " + ex.Message, LogLevel.Error);
            }
        }
    }
}
