using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class PcmService
    {
        private LoggerDebug logger = new LoggerDebug("PcmService");
        private readonly IPcmRepository pcmRepository;
        public PcmService(IPcmRepository pcmRepository)
        {
            this.pcmRepository = pcmRepository;
        }
        public async Task<bool> CreatePcm(PcmData newPcmData)
        {
            if(this.pcmRepository==null)
            {
                logger.Create("CreatePcm pcmRepository = null", LogLevel.Error);
                return false;
            }
            if(newPcmData==null)
            {
                logger.Create("CreatePcm input newPcmData = null", LogLevel.Error);
                return false;
            }
            return await this.pcmRepository.Insert(newPcmData);
        }
        public async Task<int> GetSpcCounter(int pcmResult, DateTime from, DateTime to)
        {
            if (this.pcmRepository == null)
            {
                logger.Create("GetSpcCounter pcmRepository = null", LogLevel.Error);
                return -1;
            }
            if(pcmResult < 0)
            {
                logger.Create("GetSpcCounter input pcmResult < 0", LogLevel.Error);
                return -1;
            }
            return await this.pcmRepository.SpcCounter(pcmResult, from, to);
        }

        public async Task<int> GetSpcCounterByLot(string lotId, int pcmResult, DateTime from, DateTime to)
        {
            if (this.pcmRepository == null)
            {
                logger.Create("GetSpcCounterByLot pcmRepository = null", LogLevel.Error);
                return -1;
            }
            if(string.IsNullOrEmpty(lotId))
            {
                logger.Create("GetSpcCounterByLot input lotId = null or lotId = Empty", LogLevel.Error);
                return -1;
            }
            if (pcmResult < 0)
            {
                logger.Create("GetSpcCounterByLot input pcmResult < 0", LogLevel.Error);
                return -1;
            }
            return await this.pcmRepository.SpcCounterByLot(lotId, pcmResult, from, to);
        }

        public async Task<int> GetSpcTotal(DateTime from, DateTime to)
        {
            if (this.pcmRepository == null)
            {
                logger.Create("GetSpcTotal pcmRepository = null", LogLevel.Error);
                return -1;
            }
            return await this.pcmRepository.SpcTotal(from, to);
        }

        public async Task<int> GetSpcTotalByLot(string lotId, DateTime from, DateTime to)
        {
            if (this.pcmRepository == null)
            {
                logger.Create("GetSpcTotalByLot pcmRepository = null", LogLevel.Error);
                return -1;
            }
            if(string.IsNullOrEmpty(lotId))
            {
                logger.Create("GetSpcTotalByLot input lotId = null or lotId = Empty", LogLevel.Error);
                return -1;
            }
            return await this.pcmRepository.SpcTotalByLot(lotId, from, to);
        }
    }
}
