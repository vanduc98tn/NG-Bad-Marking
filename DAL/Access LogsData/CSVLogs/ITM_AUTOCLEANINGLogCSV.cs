using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DTO;

namespace DAL
{
    public class ITM_AUTOCLEANINGLogCSV : BaseRepositoryLogs<ITM_AUTOCLEANING>
    {
        private static LoggerDebug logger = new LoggerDebug("ITM_AUTOCLEANINGLogCSV");
        private string productID;
        private string lotId;
        public ITM_AUTOCLEANINGLogCSV(string productId,string lotId)
        {
            this.productID = productId;
            this.lotId = lotId;
        }
        public override void WriteLogs(ITM_AUTOCLEANING entity)
        {
            try
            {
                // Check file existing:                    
                var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "LotCounterData");
                folder = Path.Combine(folder, DateTime.Today.ToString("yyyy-MM-dd"), this.lotId);
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
                        var header = "TIME UPDATE,PLANT,EQUIPMENT_ID,JIG_ID,CLEAN_IN_TIME,CLEAN_OUT_TIME,CLEAN_RESULT,ETC1,ETC2,ETC3,ETC4,ETC5";
                        strWriter.WriteLine(header);
                    }
                }
                // Create log:
                var log = String.Format("'{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", DateTime.Now,
                    entity.PLANT, entity.EQUIPMENT_ID, entity.JIG_ID, entity.CLEAN_IN_TIME, entity.CLEAN_OUT_TIME, entity.CLEAN_RESULT, entity.ETC1, entity.ETC2, entity.ETC3, entity.ETC4, entity.ETC5);
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
