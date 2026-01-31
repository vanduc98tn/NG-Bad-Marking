using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class MESResultDataLogService
    {
        private LoggerDebug logger = new LoggerDebug("MESResultDataLogService");
        private static Object locker = new Object();
        private IRepositoryLogs<MESResultData> repositoryLogs;
        public MESResultDataLogService(string productId, string lotId)
        {
            this.repositoryLogs = new MESResultDataLogCSV(productId, lotId);
        }
        public void CreateData(MESResultData MESdata)
        {
            if(this.repositoryLogs==null)
            {
                logger.Create("CreateData repositoryLogs = null", LogLevel.Error);
                return;
            }
            if(MESdata==null)
            {
                logger.Create("CreateData input MESdata = null", LogLevel.Error);
                return;
            }
            lock(locker)
            {
                this.repositoryLogs.WriteLogs(MESdata);
            }
        }
    }
}
