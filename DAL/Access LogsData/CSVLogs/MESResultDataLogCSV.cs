using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DTO;

namespace DAL
{
    public class MESResultDataLogCSV : BaseRepositoryLogs<MESResultData>
    {
        private static LoggerDebug logger = new LoggerDebug("MESResultDataLogCSV");
        private string productID;
        private string lotId;
        public MESResultDataLogCSV(string productId,string lotId)
        {
            this.productID = productId;
            this.lotId = lotId;
        }
        public override void WriteLogs(MESResultData entity)
        {
            try
            {
                // Check file existing:                    
                var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "LotCounterData");
                folder = Path.Combine(folder, DateTime.Today.ToString("yyyy-MM-dd"), lotId);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var fileName = String.Format("[{0}]-MES.csv", this.productID);
                var filePath = Path.Combine(folder, fileName);

                // Create Headers if file not existed:
                if (!File.Exists(filePath))
                {
                    using (var strWriter = new StreamWriter(filePath, false))
                    {
                        var header = "TIME UPDATE,LOT ID,QR CODE,MES RESULT";
                        strWriter.WriteLine(header);
                    }
                }
                if(entity.MESResult!=null && entity.MESResult.Contains(";"))
                {
                    entity.MESResult.Replace(';', ' ');
                }
                // Create log:
                var log = String.Format("'{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3}", DateTime.Now,
                    entity.LotId, entity.QrCode, entity.MESResult);
                using (var strWriter = new StreamWriter(filePath, true))
                {
                    strWriter.WriteLine(log);
                    strWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                logger.Create("WriteLogs" + ex.Message, LogLevel.Error);
            }
        }
    }
}
