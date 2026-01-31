using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DTO;

namespace DAL
{
    public class MESSendPCB : BaseRepositoryMES<MESCheck>
    {
        private LoggerDebug logger = new LoggerDebug("MESSendPCB");
        private bool isReady = false;
        public MESSendPCB(string ip, int port) : base(ip, port)
        {

        }
        public async override Task<MESCheck> Send(MESCheck entity)
        {
            this.isReceiver = false;
            try
            {
                if (!this.CheckMESConnection())
                {
                    logger.Create(" -> TCP connection not ready -> discard sending SendReady!", LogLevel.Warning);
                    return null;
                }
                var packet = new List<byte>();
                //HEADER
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(entity.EquipmentId));
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(entity.Status));
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(entity.LotNo));
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(";"));
                // BODY
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(entity.PCB_Code));
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(";"));
                //Check SUM
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(entity.CheckSum));

                var txBuf = packet.ToArray();
                logger.Create("MES.SEND PCB:" + ASCIIEncoding.ASCII.GetString(txBuf), LogLevel.Information);
                this.notifyEvenMES.NotifyToUI("Notify [MES]: SEND TO MES ->" + ASCIIEncoding.ASCII.GetString(txBuf));
                await this.SendToMes(txBuf);
                this.notifyEvenMES.NotifyToUI("Notify [MES]: Wait MES Receiver...");
                await WaitMESReturnData();
                if (this.isReceiver && !string.IsNullOrEmpty(this.DataReceiver))
                {
                    this.isReceiver = false;
                    logger.Create("MES.RECEIVER Ready:" + this.DataReceiver, LogLevel.Information);
                    return FilterData(this.DataReceiver, entity);
                }
                this.notifyEvenMES.NotifyToUI("Notify [MES]: No Response from MES");
                logger.Create("Notify [MES]: No Response from MES", LogLevel.Warning);
            }
            catch (Exception ex)
            {
                logger.Create("Send : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public async override Task<bool> SendReady(MESCheck entity)
        {
            this.isReceiver = false;
            try
            {
                if (!this.CheckMESConnection())
                {
                    logger.Create(" -> TCP connection not ready -> discard sending SendReady!", LogLevel.Warning);
                    return false;
                }
                var packet = new List<byte>();
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes(entity.EquipmentId));
                packet.AddRange(ASCIIEncoding.ASCII.GetBytes("E001"));
                var txBuf = packet.ToArray();
                logger.Create("MES.SEND Ready:" + ASCIIEncoding.ASCII.GetString(txBuf), LogLevel.Information);
                this.notifyEvenMES.NotifyToUI("Notify [MES]: SEND TO MES ->" + ASCIIEncoding.ASCII.GetString(txBuf));
                await this.SendToMes(txBuf);
                this.notifyEvenMES.NotifyToUI("Notify [MES]: Wait MES Receiver...");
                await WaitMESReturnData();
                if (this.isReceiver && !string.IsNullOrEmpty(this.DataReceiver))
                {
                    this.isReceiver = false;
                    logger.Create("MES.RECEIVER Ready:" + this.DataReceiver, LogLevel.Information);
                    var mesData = FilterData(this.DataReceiver, entity);
                    if (mesData == null) return false;
                    if (mesData.Status == "E002")
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Create("SendReady : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        private async Task WaitMESReturnData()
        {
            int counterDelayReceiver = 0;
            await Task.Run(async () => {
                while (!this.isReceiver)
                {
                    if (counterDelayReceiver > 1000)
                    {
                        break;
                    }
                    await Task.Delay(10); // Đợi 10 giây
                    counterDelayReceiver++;
                }
            });
        }
        private MESCheck FilterData(string data, MESCheck mesOld)
        {
            try
            {
                MESCheck newMESCheck = new MESCheck();
                int idex = 0;
                // EQUIPMENT ID
                string equipmentId = data.Substring(idex, 9);
                if (mesOld.EquipmentId != equipmentId)
                {
                    logger.Create("EquipmentId Is Diffrent: Old('" + mesOld.EquipmentId + "') , New('" + equipmentId + "')", LogLevel.Warning);
                    return null;
                }
                logger.Create("MES.RECEIVER EquipmentId:" + equipmentId, LogLevel.Information);
                newMESCheck.EquipmentId = equipmentId;
                idex += 9;
                // STATUS
                string status = data.Substring(idex, 4);
                if (status == "E002")
                {
                    logger.Create("MES.RECEIVER Status:" + status, LogLevel.Information);
                    newMESCheck.Status = status;
                    return newMESCheck;
                }
                logger.Create("MES.RECEIVER Status:" + status, LogLevel.Information);
                newMESCheck.Status = status;

                idex += 4;
                // LOT NO
                string lotNo = data.Substring(idex, 10);
                if (mesOld.LotNo != lotNo)
                {
                    logger.Create("LotNo Is Diffrent: Old('" + mesOld.LotNo + "') , New('" + lotNo + "')", LogLevel.Warning);
                    return null;
                }
                logger.Create("MES.RECEIVER LotNo:" + lotNo, LogLevel.Information);
                newMESCheck.LotNo = lotNo;
                idex += 10;
                //BODY
                idex += 1;
                StringBuilder stringBuilder = new StringBuilder(data);
                stringBuilder.Remove(0, idex);
                var body = stringBuilder.ToString().Split('^');
                if (body.Length >= 2)
                {
                    var x = body[1].Split(';');
                    newMESCheck.MES_Result = body[0] +"^"+ x[0];
                    if(body[0]=="OK")
                    {
                        newMESCheck.TotalCount = x[0].Length;
                        newMESCheck.MapNG = this.FillDataMAPNG(x[0]);
                    }    
                    newMESCheck.CheckSum = x[1];
                }
                else
                {
                    return null;
                }
                return newMESCheck;
            }
            catch (Exception ex)
            {
                logger.Create("FilterData : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        private List<int> FillDataMAPNG(string map)
        {
            List<int> boolList = new List<int>();
            int idex = 0;
            char bincode_NG = Convert.ToChar(SystemsManager.Instance.AppSettings.SupperOptions.BinCode_NG);
            foreach (char character in map)
            {
                idex++;
                if(character != '0')
                {
                    boolList.Add(idex);
                }
            }
            return boolList;
        }

    }
}
