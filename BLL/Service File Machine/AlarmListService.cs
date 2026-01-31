using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class AlarmListService
    {
        private LoggerDebug logger = new LoggerDebug("AlarmListService");
        private IRepositoryFileMachine<FileAlarm> fileAlarmRepository;
        public AlarmListService(IRepositoryFileMachine<FileAlarm> fileAlarmRepository)
        {
            this.fileAlarmRepository = fileAlarmRepository;
        }
        public  Dictionary<Int32, FileAlarm> GetAll()
        {
            if(this.fileAlarmRepository == null)
            {
                logger.Create("GetAll fileAlarmRepository = null", LogLevel.Error);
                return null;
            }
            return this.fileAlarmRepository.GetAll();
        }
        public Dictionary<int, FileAlarm> GetByBlock(string block)
        {
            if (this.fileAlarmRepository == null)
            {
                logger.Create("GetByBlock fileAlarmRepository = null", LogLevel.Error);
                return null;
            }
            if(string.IsNullOrEmpty(block))
            {
                logger.Create("GetByBlock input block = null or block = Empty", LogLevel.Error);
                return null;
            }
            return this.fileAlarmRepository.GetByBlock(block);
        }
        public FileAlarm GetDevice(int code)
        {
            if (this.fileAlarmRepository == null)
            {
                logger.Create("GetDevice fileAlarmRepository = null", LogLevel.Error);
                return null;
            }
            if (code<0)
            {
                logger.Create("GetDevice input code < 0", LogLevel.Error);
                return null;
            }
            return this.fileAlarmRepository.GetDevice(code);
        }

    }
}
