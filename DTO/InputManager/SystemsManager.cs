using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DTO
{
    public class SystemsManager
    {
        private LoggerDebug logger = new LoggerDebug("SystemsManager");
        private static SystemsManager instance = new SystemsManager();
        public static SystemsManager Instance => instance;

        //Notify Device
        public NotifyEvenMES NotifyEvenMES;
        public NotifyEvenCOM NotifyEvenCOM;
        public NotifyEvenTCPClient NotifyEvenTCPClient;

        //Notify PLC
        public NotifyPLCBits NotifyPLCBits;
        public NotifyPLCWord NotifyPLCWord;
        public NotifyPLCDWord NotifyPLCDWord;
        //Notify Time
        public NotifyEvenTime NotifyEvenTime;

        public AppSettings AppSettings;
        public ModelSetting currentModel { get; set; }
        private const String APP_SETTINGS_FILE_NAME = "app_settings.json";

        public bool isWriteDevice = false;

        public void StartUp()
        {
            this.LoadNotifyEven();
            this.loadAppSettings(APP_SETTINGS_FILE_NAME);
        }
        public void SaveAppSettings()
        {
            try
            {
                String filePath = Path.Combine(Directory.GetCurrentDirectory(), APP_SETTINGS_FILE_NAME);
                if (string.IsNullOrEmpty(filePath)) return;
                var js = AppSettings.ToJSON();
                if(string.IsNullOrEmpty(js)) return;
                File.WriteAllText(filePath, js);
            }
            catch (Exception ex)
            {
                logger.Create("saveAppSettings:" + ex.Message,LogLevel.Error);
            }
        }
        private void loadAppSettings(String fileName)
        {
            try
            {
                String filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), fileName);
                if (File.Exists(filePath))
                {
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        AppSettings = AppSettings.FromJSON(file.ReadToEnd());
                    }
                }
                else
                {
                    AppSettings = new AppSettings();
                }
            }
            catch (Exception ex)
            {
                logger.Create("loadAppSettings error:" + ex.Message,LogLevel.Error);
            }
        }
        private void LoadNotifyEven()
        {
            this.LoadNotifyEvenMES();
            this.LoadNotifyEvenCOM();
            this.LoadNotifyEvenTCPClient();
            this.LoadNotifyPLCBits();
            this.LoadNotifyPLCWord();
            this.LoadNotifyPLCDWord();
            this.LoadNotifyTime();
        }
        private void LoadNotifyEvenMES()
        {
            this.NotifyEvenMES = new NotifyEvenMES();
        }
        private void LoadNotifyEvenCOM()
        {
            this.NotifyEvenCOM = new NotifyEvenCOM();
        }
        private void LoadNotifyEvenTCPClient()
        {
            this.NotifyEvenTCPClient = new NotifyEvenTCPClient();
        }
        private void LoadNotifyPLCBits()
        {
            this.NotifyPLCBits = new NotifyPLCBits();
        }
        private void LoadNotifyPLCWord()
        {
            this.NotifyPLCWord = new NotifyPLCWord();
        }
        private void LoadNotifyPLCDWord()
        {
            this.NotifyPLCDWord = new NotifyPLCDWord();
        }
        private void LoadNotifyTime()
        {
            this.NotifyEvenTime = new NotifyEvenTime();
        }
        
    }
}
