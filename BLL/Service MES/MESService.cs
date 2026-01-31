using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class MESService : IObserverMES
    {
        private MESSendPCB MESSendPCB;
        public bool isAccept { get; set; }
        public string InformationClient { get; set; }
        //Notify
        private NotifyEvenMES notifyEvenMES;

        public MESService(TCPSetting tcpSetting)
        {
            this.LoadNotifyEvenMES();
            this.MESSendPCB = new MESSendPCB(tcpSetting.Ip, tcpSetting.Port);
        }
        public async Task<MESCheck> SendPCB(MESCheck entity)
        {
            if (entity.EquipmentId.Length != 9)
            {
                entity.EquipmentId = entity.EquipmentId.PadRight(9, ' ');
            }
            if (entity.LotNo.Length != 10)
            {
                entity.LotNo = entity.LotNo.PadRight(10, ' ');
            }
            if (entity.CheckSum.Length != 10)
            {
                entity.CheckSum = entity.CheckSum.PadRight(10, ' ');
            }
            return await this.MESSendPCB.Send(entity);
        }
        public async Task<bool> SendReady(MESCheck entity)
        {
            if (entity.EquipmentId.Length != 9)
            {
                entity.EquipmentId = entity.EquipmentId.PadRight(9, ' ');
            }
            return await this.MESSendPCB.SendReady(entity);
        }
        public async Task Start()
        {
            await this.MESSendPCB.Start();
            this.isAccept = this.MESSendPCB.isAccept;
        }
        public void Stop()
        {
            this.MESSendPCB.Stop();
        }
        public bool CheckConnection()
        {
            return this.MESSendPCB.CheckMESConnection();
        }
        private void LoadNotifyEvenMES()
        {
            this.notifyEvenMES = SystemsManager.Instance.NotifyEvenMES;
            this.notifyEvenMES.Attach(this);
        }
        public void CheckConnectionMES(bool connected)
        {
            this.isAccept = connected;
        }

        public void FollowingDataMES(string MESResult)
        {

        }

        public void GetInformationFromClientConnect(string clientIP, int clientPort)
        {
            this.InformationClient = clientIP + "-" + clientPort;
        }

        public void UpdateNotifyToUI(string Notify)
        {

        }
    }
}
