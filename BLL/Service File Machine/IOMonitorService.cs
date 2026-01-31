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
    public class IOMonitorService
    {
        private LoggerDebug logger = new LoggerDebug("AlarmListService");
        private IRepositoryFileMachine<FileIOMonitor> fileIORepository;
        public IOMonitorService(IRepositoryFileMachine<FileIOMonitor> fileIORepository)
        {
            this.fileIORepository = fileIORepository;
        }
        public Dictionary<Int32, FileIOMonitor> GetAll()
        {
            if (this.fileIORepository == null)
            {
                logger.Create("GetAll fileIORepository = null", LogLevel.Error);
                return null;
            }
            return this.fileIORepository.GetAll();
        }
        public Dictionary<int, FileIOMonitor> GetByBlock(string block)
        {
            if (this.fileIORepository == null)
            {
                logger.Create("GetByBlock fileIORepository = null", LogLevel.Error);
                return null;
            }
            if (string.IsNullOrEmpty(block))
            {
                logger.Create("GetByBlock input block = null or block = Empty", LogLevel.Error);
                return null;
            }
            return this.fileIORepository.GetByBlock(block);
        }
        public FileIOMonitor GetDevice(int code)
        {
            if (this.fileIORepository == null)
            {
                logger.Create("GetDevice fileIORepository = null", LogLevel.Error);
                return null;
            }
            if (code < 0)
            {
                logger.Create("GetDevice input code < 0", LogLevel.Error);
                return null;
            }
            return this.fileIORepository.GetDevice(code);
        }
    }
}
