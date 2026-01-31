using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DTO;

namespace DAL
{
    public abstract class BaseTCPClient : ITCPClient, IDisposable
    {
        private string ipServer { get; set; }
        public int portServer { get; set; }
        public bool IsConnected = false;
        private LoggerDebug logger = new LoggerDebug("BaseTCPClient");
        protected NotifyEvenTCPClient notifyEvenTCPClient;
        private CancellationTokenSource receiveCancellation;
        protected TcpClient client;
        protected NetworkStream stream;
        protected string dataReceiver;
        protected byte[] txBufReceiver;
        protected bool isReceiver;
        protected string name;
        protected int timeoutOpenInMilliseconds = 100;
        protected bool isReconnecting = false;
        protected int reconnectInterval = 5000;
        public BaseTCPClient(string name, string serverIp, int serverPort)
        {
            this.ipServer = serverIp;
            this.portServer = serverPort;
            this.name = name;
            this.LoadNotifyEvenTCPClient();
        }
        private void LoadNotifyEvenTCPClient()
        {
            this.notifyEvenTCPClient = SystemsManager.Instance.NotifyEvenTCPClient;
        }
        public void Close()
        {
            if (!this.IsConnected) return;

            this.IsConnected = false;
            this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
            this.receiveCancellation?.Cancel();
            if (this.client != null)
                this.client.Close();
            if (this.stream != null)
                this.stream.Close();
            this.notifyEvenTCPClient.NotifyToUI("Device Disconnected!!!");
        }
        public async Task<bool> Open()
        {
            try
            {
                if (this.IsConnected)
                {
                    this.notifyEvenTCPClient.NotifyToUI("Connect Device!!!");
                    return this.IsConnected;
                }
                this.Close();
                while (!this.IsConnected && !this.isReconnecting)
                {
                    using (var cts = new CancellationTokenSource(timeoutOpenInMilliseconds))
                    {
                        this.client = new TcpClient();

                        var connectTask = this.client.ConnectAsync(this.ipServer, this.portServer);

                        if (await Task.WhenAny(connectTask, Task.Delay(timeoutOpenInMilliseconds, cts.Token)) == connectTask)
                        {
                            // Kết nối thành công trong thời gian chờ
                            this.stream = this.client.GetStream();
                            if (this.client.Connected)
                            {
                                this.Receiver(); // Bắt đầu nhận dữ liệu
                                this.notifyEvenTCPClient.NotifyToUI("Connect Device OK!");
                                this.notifyEvenTCPClient.NotifyConnectionChange(this.name, true);
                                return this.IsConnected = true;
                            }
                        }
                    }
                    this.notifyEvenTCPClient.NotifyToUI("Connect Device Faild!");
                    // Đợi một khoảng thời gian trước khi thử kết nối lại
                    await Task.Delay(reconnectInterval);
                }

                this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                return false;
            }
            catch (Exception ex)
            {
                this.logger.Create("Open: " + ex.Message, LogLevel.Error);
                this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                this.IsConnected = false;
                return false;
            }
        }
        private async Task Reconnect()
        {
            if (!this.isReconnecting)
            {
                this.isReconnecting = false;
                await this.Open();
                this.isReconnecting = true;
            }
        }

        public void Receiver()
        {
            try
            {
                if (this.client == null || !client.Connected)
                {
                    this.logger.Create("Receiver Client is not connected ", LogLevel.Warning);
                    this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                    return;
                }
                byte[] buffer = new byte[1024];
                //StringBuilder receivedData = new StringBuilder();
                this.receiveCancellation = new CancellationTokenSource();
                Task.Run(async () => {
                    while (!receiveCancellation.Token.IsCancellationRequested)
                    {
                        if (this.client.Connected && stream.DataAvailable)
                        {
                            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                this.IsConnected = false;
                                this.notifyEvenTCPClient.NotifyConnectionChange(this.name, false);
                                this.logger.Create("Receiver Client Disconnect!!!", LogLevel.Warning);
                                this.Dispose();
                                await this.Reconnect();
                                break;
                            }
                            this.txBufReceiver = buffer;
                            string receivedChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            this.isReceiver = true;
                            //receivedData.Append(receivedChunk);
                            this.dataReceiver = receivedChunk;
                        }
                        else if (!this.client.Connected)
                        {
                            this.IsConnected = false;
                            await this.Reconnect();
                            break;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                this.notifyEvenTCPClient.NotifyToUI("Receiver error: " + ex.Message);
                this.logger.Create("Receiver: " + ex.Message, LogLevel.Error);
            }
        }

        public abstract Task Send(string dataSend);

        public void Dispose()
        {
            this.Close();
        }
    }
}
