using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DTO;

namespace DAL
{
    public class AlarmList : BaseRepositoryFileMachine<FileAlarm>
    {
        private static Dictionary<Int32, FileAlarm> alarmLists = new Dictionary<Int32, FileAlarm>();
        private LoggerDebug logger = new LoggerDebug("AlarmList");
        private string alarmFilePath;
        public AlarmList(string alarmFilePath)
        {
            this.alarmFilePath = alarmFilePath;
            this.LoadFileMachine(this.alarmFilePath);
        }

        public override Dictionary<Int32, FileAlarm> GetAll()
        {
            try
            {
                if (alarmLists == null)
                {
                    logger.Create("GetAll alarmLists = null", LogLevel.Warning);
                    return null;
                }
                return alarmLists;
            }
            catch(Exception ex)
            {
                logger.Create("GetAll : " + ex.Message,LogLevel.Error);
            }
            return null;
        }

        public override Dictionary<int, FileAlarm> GetByBlock(string block)
        {
            try
            {
                if (alarmLists == null)
                {
                    logger.Create("GetByBlock alarmLists = null", LogLevel.Warning);
                    return null;
                }
                Dictionary<int, FileAlarm> newBlock = new Dictionary<int, FileAlarm>();
                foreach(var x in alarmLists)
                {
                    if(x.Value.Block== block || x.Value.DeviceCode>=0)
                    {
                        newBlock.Add(x.Key,x.Value);
                    }
                }
                return newBlock;
            }
            catch(Exception ex)
            {
                logger.Create("GetByBlock : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public override FileAlarm GetDevice(int code)
        {
            try
            {
                if (alarmLists == null)
                {
                    logger.Create("GetDevice alarmLists = null", LogLevel.Warning);
                    return null;
                }
                if (!alarmLists.ContainsKey(code))
                {
                    logger.Create("GetDevice Not Exist : " + code + " In alarmLists", LogLevel.Warning);
                    return null;
                }
                FileAlarm alarm;
                alarmLists.TryGetValue(code,out alarm);
                return alarm;
            }
            catch(Exception ex)
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
                    if(string.IsNullOrEmpty(text[1]))
                    {
                        continue;
                    }
                    int number;
                    if (!int.TryParse(text[1], out number)) continue;
                    alarmLists.Add(Convert.ToInt32(text[1]), new FileAlarm
                    {
                        Device = text[0].ToString(),
                        DeviceCode = Convert.ToInt32(text[1]),
                        Block = text[2].ToString(),
                        Message = text[3].ToString(),
                        Solution = text[4].ToString()
                    });
                }
                return true;
            }
            catch(Exception ex)
            {
                logger.Create("LoadFileMachine : " + ex.Message, LogLevel.Error);
            }   
            return false;
        }
    }
}
