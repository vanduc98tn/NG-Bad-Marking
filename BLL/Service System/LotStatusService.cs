using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class LotStatusService
    {
        private LoggerDebug logger = new LoggerDebug("LotStatusService");
        private readonly ILotStatusRepository lotStatusRepository;
        public LotStatusService(ILotStatusRepository lotStatusRepository)
        {
            this.lotStatusRepository = lotStatusRepository;
        }
        public async Task<bool> CreateLot(LotStatus newLot)
        {
            if(this.lotStatusRepository==null)
            {
                logger.Create("CreateLot lotStatusRepository = null", LogLevel.Error);
                return false;
            }
            if(newLot == null)
            {
                logger.Create("CreateLot input newLot = null", LogLevel.Error);
                return false;
            }
            return await this.lotStatusRepository.Insert(newLot);
        }
        public async Task<bool> UpdateLot(LotStatus editLot)
        {
            if (this.lotStatusRepository == null)
            {
                logger.Create("UpdateLot lotStatusRepository = null", LogLevel.Error);
                return false;
            }
            if (editLot == null)
            {
                logger.Create("UpdateLot input editLot = null", LogLevel.Error);
                return false;
            }
            return await this.lotStatusRepository.Update(editLot);
        }
        public async Task<IEnumerable<LotStatus>> GetAllLotStatus()
        {
            return await this.lotStatusRepository.GetAll();
        }
        public async Task<LotStatus> GetLotStatus(string lotId)
        {
            if (this.lotStatusRepository == null)
            {
                logger.Create("GetLotStatus lotStatusRepository = null", LogLevel.Error);
                return null;
            }
            if(string.IsNullOrEmpty(lotId))
            {
                logger.Create("GetLotStatus input lotId = null or lotId = Empty", LogLevel.Error);
                return null;
            }
            return await this.lotStatusRepository.GetLotStatusByLotId(lotId);
        }
        public async Task<LotStatus> GetCurrentLotStatus()
        {
            if (this.lotStatusRepository == null)
            {
                logger.Create("GetCurrentLotStatus lotStatusRepository = null", LogLevel.Error);
                return null;
            }
            return await this.lotStatusRepository.GetMostRecent();
        }
        public async Task<IEnumerable<LotStatus>> GetLotStatusByTime(DateTime from, DateTime to)
        {
            if (this.lotStatusRepository == null)
            {
                logger.Create("GetLotStatusByTime lotStatusRepository = null", LogLevel.Error);
                return null;
            }
            return await this.lotStatusRepository.GetLotStatusByTime(from, to);
        }
    }
}
