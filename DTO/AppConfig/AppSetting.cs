using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DTO
{
    public class AppSettings
    {
        public DeviceType CurrentDeviceType;
        public string currentModel;
        public SettingDevice SettingDevice;
        public LotInData LotInData;
        public RunSetting RunSetting;
        public SuperOptions SupperOptions;
        public FilePathSetting FilePathSetting;
        public ItemAssign ItemAssign;
        public JIGAoiOffline JigAoiOffline;
        public TCPSetting AOISetting;
        public ScannerCOMSetting ScannerSetting;
        public TimeInfor TimeInfor;
        public MESSetting MESSetting;

        public List<CircleInfor> CircleInfors;
        public List<ImageInfor> ImageInfors;
        public AppSettings()
        {
            this.SettingDevice = new SettingDevice();
            this.LotInData = new LotInData();
            this.RunSetting = new RunSetting();
            this.SupperOptions = new SuperOptions();
            this.CurrentDeviceType = DeviceType.MCProtocolBinaryEthernet;
            this.FilePathSetting = new FilePathSetting();
            this.ItemAssign = new ItemAssign();
            this.AOISetting = new TCPSetting();
            this.JigAoiOffline = new JIGAoiOffline();
            this.currentModel = "default";
            this.TimeInfor = new TimeInfor();
            this.MESSetting = new MESSetting();
            this.ScannerSetting = new ScannerCOMSetting();
            this.CircleInfors = new List<CircleInfor>();
            this.ImageInfors = new List<ImageInfor>();
        }
        public String ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static AppSettings FromJSON(String js)
        {
            var j = JsonConvert.DeserializeObject<AppSettings>(js);
            if(j.SettingDevice==null)
            {
                j.SettingDevice = new SettingDevice();
            }
            if(j.LotInData==null)
            {
                j.LotInData = new LotInData();
            }
            if(j.RunSetting==null)
            {
                j.RunSetting = new RunSetting();
            }
            if (j.CircleInfors == null)
            {
                j.CircleInfors = new List<CircleInfor>();
            }
            if (j.ImageInfors == null)
            {
                j.ImageInfors = new List<ImageInfor>();
            }
            if (j.SupperOptions==null)
            {
                j.SupperOptions = new SuperOptions();
            }
            if (j.ItemAssign == null)
            {
                j.ItemAssign = new ItemAssign();
            }
            if(j.AOISetting==null)
            {
                j.AOISetting = new TCPSetting();
            }
            if(j.TimeInfor==null)
            {
                j.TimeInfor = new TimeInfor();
            }
            if(j.MESSetting==null)
            {
                j.MESSetting = new MESSetting();
            }
            if(j.JigAoiOffline==null)
            {
                j.JigAoiOffline = new JIGAoiOffline();
            }
            j.FilePathSetting = new FilePathSetting();
            if(j.currentModel=="")
            {
                j.currentModel = "default";
            }
            if(j.ScannerSetting==null)
            {
                j.ScannerSetting = new ScannerCOMSetting();
            }
            return j;
        }
    }
}
