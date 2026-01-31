using DTO;
using DAL;

namespace BLL
{
    public class IOList
    {
        public IRepositoryFileMachine<FileIOMonitor> FileIORepository;
        public IOList()
        {
            var strFile = SystemsManager.Instance.AppSettings.FilePathSetting.FilePathMonitorIO;
            this.FileIORepository = new DAL.IOList(strFile);
        }
    }
}
