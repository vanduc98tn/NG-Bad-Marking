using DAL;
using DTO;

namespace BLL
{
    public class AlarmList
    {
        public IRepositoryFileMachine<FileAlarm> FileAlarmRepository;
        public AlarmList()
        {
            var strFile = SystemsManager.Instance.AppSettings.FilePathSetting.FilePathAlarmList;
            this.FileAlarmRepository = new DAL.AlarmList(strFile);
        }
    }
}
