using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DTO;

namespace DAL
{
    public class IOList : BaseRepositoryFileMachine<FileIOMonitor>
    {
        private static Dictionary<Int32, FileIOMonitor> ioLists = new Dictionary<Int32, FileIOMonitor>();
        private LoggerDebug logger = new LoggerDebug("IOList");
        private string ioListFilePath;
        public IOList(string ioListFilePath)
        {
            this.ioListFilePath = ioListFilePath;
            this.LoadFileMachine(this.ioListFilePath);
        }
        public override Dictionary<int, FileIOMonitor> GetByBlock(string block)
        {
            try
            {
                if (ioLists == null)
                {
                    logger.Create("GetByBlock alarmLists = null", LogLevel.Warning);
                    return null;
                }
                Dictionary<int, FileIOMonitor> newBlock = new Dictionary<int, FileIOMonitor>();
                foreach (var x in ioLists)
                {
                    if (x.Value.Block == block || x.Value.DeviceCode >= 0)
                    {
                        newBlock.Add(x.Key, x.Value);
                    }
                }
                return newBlock;
            }
            catch (Exception ex)
            {
                logger.Create("GetByBlock : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override Dictionary<int, FileIOMonitor> GetAll()
        {
            try
            {
                if (ioLists == null)
                {
                    logger.Create("GetAll ioLists = null", LogLevel.Warning);
                    return null;
                }
                return ioLists;
            }
            catch (Exception ex)
            {
                logger.Create("GetAll : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public override FileIOMonitor GetDevice(int code)
        {
            try
            {
                if (ioLists == null)
                {
                    logger.Create("GetDevice ioLists = null", LogLevel.Warning);
                    return null;
                }
                if (!ioLists.ContainsKey(code))
                {
                    logger.Create("GetDevice Not Exist : " + code + " In ioLists", LogLevel.Warning);
                    return null;
                }
                FileIOMonitor ioList;
                ioLists.TryGetValue(code, out ioList);
                return ioList;
            }
            catch (Exception ex)
            {
                logger.Create("GetDevice : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public override bool LoadFileMachine(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    string[] text = line.Split(',');
                    if (string.IsNullOrEmpty(text[1]))
                    {
                        continue;
                    }
                    int number;
                    if (!int.TryParse(text[1], out number)) continue;
                    ioLists.Add(Convert.ToInt32(text[1]), new FileIOMonitor
                    {
                        Device = text[0].ToString(),
                        DeviceCode = Convert.ToInt32(text[1]),
                        Block = text[2].ToString(),
                        Description = text[3].ToString()
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Create("LoadFileMachine : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
    }
}
