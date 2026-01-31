using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using DTO;
namespace DAL
{
    public abstract class BaseCOM : ICOM, IDisposable
    {
        private LoggerDebug logger = new LoggerDebug("BaseCOM");
        protected NotifyEvenCOM notifyEvenCOM;
        protected COMSetting comSetting;
        public bool IsConnected = false;
        protected SerialPort serialPort;
        protected CancellationTokenSource receiveCancellation;
        protected string dataReceiver;
        protected bool isReceiver;
        protected byte[] txBufReceiver;
        protected string name;
        protected int timeoutOpenInMilliseconds = 100;
        protected Handshake Handshake;

        public BaseCOM(string name,COMSetting comSetting,Handshake hand = Handshake.None)
        {
            this.comSetting = comSetting;
            this.serialPort = new SerialPort(comSetting.portName, comSetting.baudrate, comSetting.parity,
                comSetting.dataBits, comSetting.stopBits);
            this.serialPort.Handshake = hand;
            this.name = name;
            this.LoadNotifyEvenScannerCOM();
            this.serialPort.DataReceived += SerialPort_DataReceived;
        }
        public virtual async Task<bool> Open()
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    var openTask = Task.Run(() =>
                    {
                        serialPort.Open();
                        this.notifyEvenCOM.CheckConnectChange(this.name, true);
                        this.IsConnected = serialPort.IsOpen;
                        this.notifyEvenCOM.NotifyToUI("Opened Scanner Complete!!!");
                    });
                    openTask.Wait();
                    if (await Task.WhenAny(openTask, Task.Delay(timeoutOpenInMilliseconds)) == openTask)
                    {
                        var x = openTask.IsCompleted && !openTask.IsFaulted;
                        return x;
                    }
                    else
                    {
                        // Timeout
                        serialPort.Close();
                        this.notifyEvenCOM.CheckConnectChange(this.name, false);
                        this.notifyEvenCOM.NotifyToUI("Open '"+ this.name + "' Timeout");
                        this.IsConnected = false;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Create("Open: " + ex.Message, LogLevel.Error);
                    this.notifyEvenCOM.NotifyToUI("Open Serial Error: " + ex.Message);
                    this.IsConnected = false;
                    return false;
                }
            }
            this.notifyEvenCOM.CheckConnectChange(this.name, true);
            return true;
        }
        private void LoadNotifyEvenScannerCOM()
        {
            this.notifyEvenCOM = SystemsManager.Instance.NotifyEvenCOM;
        }
        public virtual async void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                if (serialPort.IsOpen)
                {
                    bytesRead = await serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (bytesRead > 0)
                {
                    this.txBufReceiver = buffer;
                    var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    this.notifyEvenCOM.NotifyResultUI(this.name, receivedData);
                    if (!string.IsNullOrEmpty(receivedData))
                    {
                        this.notifyEvenCOM.NotifyResultUI(this.name, receivedData);
                        this.dataReceiver = receivedData;
                        this.isReceiver = true;
                    }
                }
                else
                {
                    this.IsConnected = false;
                    this.notifyEvenCOM.CheckConnectChange(this.name, false);
                }
            }
            catch(Exception ex)
            {
                logger.Create("SerialPort_DataReceived: " + ex.Message, LogLevel.Error);
            }
            
        }
        public abstract Task Send(byte[] txBuf);

        public void Stop()
        {
            if (serialPort.IsOpen)
            {
                this.IsConnected = false;
                this.notifyEvenCOM.CheckConnectChange(this.name, false);
                serialPort.Close();
                receiveCancellation?.Cancel();
                this.notifyEvenCOM.NotifyToUI("Stoped '"+this.name+"' Complete!!!");
            }
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
