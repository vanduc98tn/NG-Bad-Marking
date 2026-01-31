using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class ITM_AutoCleaningService
    {
        private LoggerDebug logger = new LoggerDebug("ITM_AutoCleaningService");
        private IITM_AUTOCLEANING itmAutoCleaning;
        public ITM_AutoCleaningService(IITM_AUTOCLEANING itmAutoCleaning)
        {
            this.itmAutoCleaning = itmAutoCleaning;
        }
        public async Task<bool> Exists(string jigID)
        {
            if(this.itmAutoCleaning==null)
            {
                logger.Create("Exists itmAutoCleaning  = null", LogLevel.Error);
                return false;
            }
            if(string.IsNullOrEmpty(jigID))
            {
                logger.Create("Exists input jigID = null or jigID = Empty", LogLevel.Error);
                return false;
            }
            return await this.itmAutoCleaning.Exists(jigID);
        }
        public async Task<bool> AddProduct(ITM_AUTOCLEANING AddProduct)
        {
            if (this.itmAutoCleaning == null)
            {
                logger.Create("AddProduct itmAutoCleaning  = null", LogLevel.Error);
                return false;
            }
            if(AddProduct == null)
            {
                logger.Create("AddProduct input AddProduct  = null", LogLevel.Error);
                return false;
            }
            return await this.itmAutoCleaning.Insert(AddProduct);
        }
        public async Task<bool> UpdateInTime(string jigID,string inTime)
        {
            if (this.itmAutoCleaning == null)
            {
                logger.Create("UpdateInTime itmAutoCleaning  = null", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(jigID))
            {
                logger.Create("UpdateInTime input jigID  = null or jigID = Empty", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(inTime))
            {
                logger.Create("UpdateInTime input inTime  = null or inTime = Empty", LogLevel.Error);
                return false;
            }
            return await this.itmAutoCleaning.UpdateInTime(jigID, inTime);
        }
        public async Task<bool> UpdateOutTime(string jigID, string outTime)
        {
            if (this.itmAutoCleaning == null)
            {
                logger.Create("UpdateOutTime itmAutoCleaning  = null", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(jigID))
            {
                logger.Create("UpdateOutTime input jigID  = null or jigID = Empty", LogLevel.Error);
                return false;
            }
            if (string.IsNullOrEmpty(outTime))
            {
                logger.Create("UpdateOutTime input outTime  = null or outTime = Empty", LogLevel.Error);
                return false;
            }
            return await this.UpdateOutTime(jigID, outTime);
        }
    }
}
