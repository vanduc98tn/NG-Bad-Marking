using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLManager
    {
        private LoggerDebug logger = new LoggerDebug("BLLManager");
        private static BLLManager instance = new BLLManager();
        public static BLLManager Instance => instance;
        public bool isConnectPLC = false;
        public bool DeviceLoadBit_M = false;
        public bool DeviceLoadBit_X = false;
        public bool DeviceLoadBit_Y = false;
        public bool DeviceLoadBit_L = false;
        public bool DeviceLoadWord = false;
        public bool DeviceLoadDWord = false;
        public DeviceFactory PLC;
        public ServiceScannerCOM scannerCOM;
        public ServiceScannerTCP scannerTCP;
        public MESService MES;
        public ServiceAOI serviceAOI;
        public IOMonitorService IoMonitorService;
        public AlarmListService AlarmListService;
        public ServiceModel ServiceModel;
        public ServiceTime ServiceTime;
        

        public async Task LoadPLC()
        {
            try
            {
                this.PLC = new DeviceFactory(SystemsManager.Instance.AppSettings.CurrentDeviceType, SystemsManager.Instance.AppSettings.SettingDevice);
                var tsk = Task.Run(async () => {
                    this.isConnectPLC = await this.PLC.Device.Open();
                });
                await Task.Delay(1);
                
                this.PLC.MonitorDevice();
            }
            catch (Exception ex)
            {
                logger.Create("LoadPLC: " + ex.Message, LogLevel.Error);
            }
        }
        public async Task LoadFile()
        {
            try
            {
                var tsk = Task.Run(() =>
                {
                    IOList ioMonitor = new IOList();
                    AlarmList alarmList = new AlarmList();
                    this.IoMonitorService = new IOMonitorService(ioMonitor.FileIORepository);
                    this.AlarmListService = new AlarmListService(alarmList.FileAlarmRepository);
                });
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                logger.Create("LoadFile: " + ex.Message, LogLevel.Error);
            }
        }
        public void LoadTimeRunMachine()
        {
            this.ServiceTime = new ServiceTime();
        }
        public async Task LoadAOI()
        {
            this.serviceAOI = new ServiceAOI(SystemsManager.Instance.AppSettings.AOISetting);
            var tsk = Task.Run(async () => {
                var connect = await this.serviceAOI.Open();
            });
            await Task.Delay(1);
        }
        public void LoadModel()
        {
            this.ServiceModel = new ServiceModel();
            var model = this.ServiceModel.GetModelSettings(SystemsManager.Instance.AppSettings.currentModel);
            if (model == null)
            {
                model = new ModelSetting();
            }
            SystemsManager.Instance.currentModel = model;
            this.SaveModel();
        }
        public void SaveModel()
        {
            this.ServiceModel.UpdateModelSettings(SystemsManager.Instance.currentModel);
        }
        public async Task LoadScanner()
        {
            this.scannerCOM = new ServiceScannerCOM(SystemsManager.Instance.AppSettings.ScannerSetting);
            await Task.Run(async () => {
                var connect = await this.scannerCOM.Open();
            });
        }
        public async Task LoadScannerTCP()
        {
            this.scannerTCP = new ServiceScannerTCP(SystemsManager.Instance.AppSettings.SettingDevice.ScannerTCPSetting);
            var tsk = Task.Run(async () => {
                var connect = await this.scannerTCP.Open();
            });
            await Task.Delay(1);
        }
        public async Task LoadMES()
        {
            this.MES = new MESService(SystemsManager.Instance.AppSettings.MESSetting);
            var tsk = Task.Run(async () => {
                await this.MES.Start();
            });
            await Task.Delay(1);
        }
    }
}
