using System;
using System.IO;

namespace DTO
{
    public class FilePathSetting
    {
        public string SqlLiteConnectString { get; set; }
        public string SqlServerConnectString { get; set; }
        public string FilePathMonitorIO { get; set; }
        public string FilePathAlarmList { get; set; }
        public FilePathSetting()
        {
            this.SqlLiteConnectString = this.GetFileDB("DataSystem.db");
            this.SqlServerConnectString = "123";
            this.FilePathMonitorIO = this.GetFilePath("IOList.csv");
            this.FilePathAlarmList = this.GetFilePath("alarmList.csv");
        }
        private string GetFilePath(string nameFile)
        {
            return System.IO.Path.Combine(Directory.GetCurrentDirectory(), nameFile);
        }
        private string GetFileDB(string nameFile)
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), nameFile);
            return String.Format("Data Source={0};Mode=ReadWrite;", dbPath);
        }
    }
}
