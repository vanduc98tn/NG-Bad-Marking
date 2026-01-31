using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using DTO;

namespace DAL
{
    public abstract class BaseRepositoryMES<TEntity> : IRepositoryMES<TEntity>
    {
        private LoggerDebug logger = new LoggerDebug("BaseRepositoryMES");
        //Notify
        protected NotifyEvenMES notifyEvenMES;
        // TCP server:
        protected TcpListener listener;
        protected TcpClient mesClients;
        protected bool isRunning = false;
        public bool isAccept = false;
        //
        protected bool isReceiver = false;
        protected string DataReceiver { get; set; }

        public BaseRepositoryMES(string ip, int port)
        {
            try
            {
                this.LoadNotifyEvenMES();
                this.listener = new TcpListener(IPAddress.Parse(ip), port);
            }
            catch (Exception ex)
            {
                logger.Create("BaseRepositoryMES :" + ex.Message, LogLevel.Error);
            }
        }
        private void LoadNotifyEvenMES()
        {
            this.notifyEvenMES = SystemsManager.Instance.NotifyEvenMES;
        }
        public async Task Start()
        {
            if (this.isRunning)
            {
                this.notifyEvenMES.NotifyToUI("Notify : MES Is Opend!!!");
                return;
            }
            try
            {
                this.listener.Start();
                this.isRunning = true;
                this.notifyEvenMES.NotifyToUI("Notify : Listen MES...!!!");
                while (isRunning)
                {
                    TcpClient mesClient = await listener.AcceptTcpClientAsync();
                    if (mesClients == null)
                    {
                        mesClients = mesClient;
                        this.notifyEvenMES.NotifyToUI("Notify : Server Connected To MES!!!");
                        this.notifyEvenMES.NotifyMESConnect(true);
                        // Lấy địa chỉ IP của client
                        string clientIP = ((IPEndPoint)mesClient.Client.RemoteEndPoint).Address.ToString();
                        int clientPort = ((IPEndPoint)mesClient.Client.RemoteEndPoint).Port;
                        this.notifyEvenMES.GetInformationFromClientConnect(clientIP, clientPort);
                        this.isAccept = true;
                        this.notifyEvenMES.NotifyToUI($"Notify : Client connected from {clientIP}:{clientPort}");
                        await Task.Run(() => HandleMesClient(mesClients));
                    }
                    else
                    {
                        this.notifyEvenMES.NotifyToUI("Notify : MES Is Disconnect!!!");
                        this.notifyEvenMES.NotifyMESConnect(false);
                        this.isAccept = false;
                        mesClient.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                this.notifyEvenMES.NotifyToUI("Notify : MES Connect Faild " + ex.Message);
                this.logger.Create("MES Connect Faild " + ex.Message, LogLevel.Error);
                this.notifyEvenMES.NotifyMESConnect(false);
                this.isAccept = false;
            }
        }
        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                listener.Stop();
                mesClients.Close();
                mesClients = null;
                this.notifyEvenMES.NotifyToUI("Notify : Server Is Closed!!!");
                this.notifyEvenMES.NotifyMESConnect(false);
                this.isAccept = false;
            }
        }
        public bool CheckMESConnection()
        {
            return mesClients != null && mesClients.Connected;
        }
        public async Task HandleMesClient(TcpClient mesClient)
        {
            NetworkStream stream = mesClient.GetStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        this.notifyEvenMES.NotifyToUI("Notify : Connect From MES Is Closed!!!");
                        this.notifyEvenMES.NotifyMESConnect(false);
                        this.isAccept = false;
                        mesClient.Close();
                        await ReconnectToMES();
                        break;
                    }
                    string requestData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string responseData = requestData;
                    if(!responseData.Contains("E001"))
                    {
                        this.DataReceiver = responseData;
                        this.isReceiver = true;
                    }    
                    this.notifyEvenMES.NotifyMESResult(responseData);
                }
                catch (Exception ex)
                {
                    this.notifyEvenMES.NotifyToUI("Notify : Error Access MES : " + ex.Message);
                    this.notifyEvenMES.NotifyMESConnect(false);
                    this.isAccept = false;
                    mesClient.Close();
                    isRunning = false;
                    mesClients = null;
                    break;
                }
            }
        }
        public async Task SendToMes(byte[] txBuf)
        {
            if (!isRunning)
            {
                this.notifyEvenMES.NotifyToUI("Notify : Server Is Closed or MES is Closed : ");
                this.notifyEvenMES.NotifyMESConnect(false);
                this.isAccept = false;
                return;
            }
            NetworkStream stream = mesClients.GetStream();
            await stream.WriteAsync(txBuf, 0, txBuf.Length);
            byte[] buffer = new byte[1024];
        }
        private async Task ReconnectToMES()
        {
            isRunning = false;
            mesClients = null;
            await Start();
        }

        public abstract Task<TEntity> Send(TEntity entity);

        public abstract Task<bool> SendReady(TEntity entity);
    }
}
