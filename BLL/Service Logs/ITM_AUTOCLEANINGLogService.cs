using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class ITM_AUTOCLEANINGLogService
    {
        private LoggerDebug logger = new LoggerDebug("ITM_AUTOCLEANINGLogService");
        private static Object locker = new Object();
        private IRepositoryLogs<ITM_AUTOCLEANING> repositoryLogs;
        public ITM_AUTOCLEANINGLogService(string productId, string lotId)
        {
            this.repositoryLogs = new ITM_AUTOCLEANINGLogCSV(productId, lotId);
        }
        public void CreateData(ITM_AUTOCLEANING ITM_AUTOCLEANINGdata)
        {
            if(this.repositoryLogs == null)
            {
                logger.Create("CreateData repositoryLogs = null", LogLevel.Error);
                return;
            }
            if(ITM_AUTOCLEANINGdata == null)
            {
                logger.Create("CreateData input ITM_AUTOCLEANINGdata = null", LogLevel.Error);
                return;
            }
            lock(locker)
            {
                this.repositoryLogs.WriteLogs(ITM_AUTOCLEANINGdata);
            }
        }
    }
}
