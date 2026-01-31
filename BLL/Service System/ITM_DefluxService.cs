using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class ITM_DefluxService
    {
        private LoggerDebug logger = new LoggerDebug("ITM_DefluxService");
        private IITM_DEFLUX IitmDeflux;
        public ITM_DefluxService(IITM_DEFLUX IitmDeflux)
        {
            this.IitmDeflux = IitmDeflux;
        }
        public async Task<bool> AddDeflux(ITM_DEFLUX itmDeflux)
        {
            if(this.IitmDeflux==null)
            {
                logger.Create("AddDeflux IitmDeflux = null", LogLevel.Error);
                return false;
            }
            if(itmDeflux==null)
            {
                logger.Create("AddDeflux input itmDeflux = null", LogLevel.Error);
                return false;
            }
            return await this.IitmDeflux.Insert(itmDeflux);
        }
        public async Task<bool> Exists(string jigId)
        {
            if (this.IitmDeflux == null)
            {
                logger.Create("Exists IitmDeflux = null", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(jigId))
            {
                logger.Create("Exists input jigId = null or jigId = Empty", LogLevel.Error);
                return false;
            }
            return await this.IitmDeflux.Exists(jigId);
        }
        public async Task<bool> RemoveDeflux(string jigId)
        {
            if (this.IitmDeflux == null)
            {
                logger.Create("RemoveDeflux IitmDeflux = null", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(jigId))
            {
                logger.Create("RemoveDeflux input jigId = null or jigId = Empty", LogLevel.Error);
                return false;
            }
            return await this.IitmDeflux.DeleteByJIG(jigId);
        }
        public async Task<bool> UpdateOutTime(string jigID, string outTime)
        {
            if (this.IitmDeflux == null)
            {
                logger.Create("UpdateOutTime IitmDeflux = null", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(jigID))
            {
                logger.Create("UpdateOutTime input jigID = null or jigID = Empty", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(outTime))
            {
                logger.Create("UpdateOutTime input outTime = null or outTime = Empty", LogLevel.Error);
                return false;
            }
            return await this.IitmDeflux.UpdateOutTime(jigID, outTime);
        }
    }
}
