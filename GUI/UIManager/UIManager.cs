using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using DTO;
using BLL;
using System.Threading;

namespace GUI
{
    public enum PAGE_ID
    {
        PAGE_MAIN = 0,
        PAGE_MENU,
            PAGE_MENU_MANUAL_PICKER_TRANSFER,
            PAGE_MENU_MANUAL_BUFFER_PICKER_HANDLE,
            PAGE_MENU_MANUAL_MOTOR,
            PAGE_MENU_MANUAL_CENTERING,
                PAGE_MENU_MECHANICAL_DEVICE,
                PAGE_MENU_MECHANICAL_DELAY_TIME,
                PAGE_MENU_MECHANICAL_AOI,
                PAGE_MENU_MECHANICAL_BARCODE,
            PAGE_MENU_MODEL,
            PAGE_MENU_LOAD,
            PAGE_MENU_STATUS,
            PAGE_MENU_SUPER_USER,
            PAGE_MENU_SYSTEM,
            PAGE_MENU_TEACHING,
            PAGE_MENU_ASSIGN,
        PAGE_IO,
        PAGE_LAST_JAM
    }
    class UIManager
    {
        private static UIManager instance = new UIManager();
        public static UIManager Instance => instance;
        private LoggerDebug logger = new LoggerDebug("UIManager");
        public Hashtable pageTable = new Hashtable();
        private MainWindow wndMain;
        public bool isShowAlarm;
        //public DeviceFactory PLC;
        public bool isConnectDevice = false;

        public async Task StartupUI()
        {
            logger.Create("Startup:",LogLevel.Information);
            try
            {
                await this.LoadMachine();
                await this.LoadMES();
                await this.LoadDevicePLC();
                //await this.LoadDeviceAOI();
                //await this.LoadScanner();
                await this.LoadScannerTCP();
                this.LoadTimeRunMachine();
                this.LoadModel();
                this.initPages();
                this.LoadMainWindow();
            }
            catch (Exception ex)
            {
                logger.Create("Startup error:" + ex.Message,LogLevel.Error);
            }
        }
        private void LoadMainWindow()
        {
            // Create Main window:
            this.wndMain = new MainWindow();
            // Create all pages and add to the local table:
            wndMain.mainContent.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            wndMain.Show();
        }
        public Boolean IsRunningAuto()
        {
            var pg = (PgMain)pageTable[PAGE_ID.PAGE_MAIN];
            if (pg != null)
            {
                return pg.IsRunningAuto();
            }
            return false;
        }
        public void SwitchPage(PAGE_ID pgId)
        {
            if (pageTable.ContainsKey(pgId))
            {
                var pg = (System.Windows.Controls.Page)pageTable[pgId];
                wndMain.UpdateMainContent(pg);
            }
            // Update Main status bar:
            if (pgId == PAGE_ID.PAGE_MAIN)
            {
                wndMain.btMain.Background = Brushes.Orange;
            }
            else
            {
                wndMain.btMain.ClearValue(Button.BackgroundProperty);
            }
            if (pgId >= PAGE_ID.PAGE_MENU && pgId <= PAGE_ID.PAGE_MENU_ASSIGN)
            {
                wndMain.btMenu.Background = Brushes.Orange;
            }
            else
            {
                wndMain.btMenu.ClearValue(Button.BackgroundProperty);
            }
            if (pgId == PAGE_ID.PAGE_IO)
            {
                wndMain.btIO.Background = Brushes.Orange;
            }
            else
            {
                wndMain.btIO.ClearValue(Button.BackgroundProperty);
            }
            if (pgId == PAGE_ID.PAGE_LAST_JAM)
            {
                wndMain.btLastJam.Background = Brushes.Orange;
            }
            else
            {
                wndMain.btLastJam.ClearValue(Button.BackgroundProperty);
            }
        }
        private void initPages()
        {
            this.pageTable.Add(PAGE_ID.PAGE_MAIN, new PgMain());
            this.pageTable.Add(PAGE_ID.PAGE_MENU, new PgMenuBase());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MANUAL_PICKER_TRANSFER, new PgManualPickerTransferMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MANUAL_BUFFER_PICKER_HANDLE, new PgManualPickerBufferPickerHandle());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MANUAL_MOTOR, new PgManualMotor());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MANUAL_CENTERING, new PgManualCentering());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MECHANICAL_DEVICE, new PgMechanicalPLCMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MECHANICAL_AOI, new PgAOISettingMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MECHANICAL_BARCODE, new PgMechanicalBarcode());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MECHANICAL_DELAY_TIME, new PgMechanicalDelayTime());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_MODEL, new PgModelMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_LOAD, new PgMenuLoad());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_STATUS, new PgStatusMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_SUPER_USER, new PgSuperUserMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_SYSTEM, new PgSystemMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_TEACHING, new PgTeachingMenu());
            this.pageTable.Add(PAGE_ID.PAGE_MENU_ASSIGN, new PgAssignMenu());
            this.pageTable.Add(PAGE_ID.PAGE_IO, new PgIO());
            this.pageTable.Add(PAGE_ID.PAGE_LAST_JAM, new PgLastAlarm());
        }
        
        private async Task LoadDevicePLC()
        {
            await BLLManager.Instance.LoadPLC();
        }
        private async Task LoadMachine()
        {
            await BLLManager.Instance.LoadFile();
        }
        private async Task LoadDeviceAOI()
        {
            await BLLManager.Instance.LoadAOI();
        }
        private async Task LoadScanner()
        {
            await BLLManager.Instance.LoadScanner();
        }
        private async Task LoadScannerTCP()
        {
            await BLLManager.Instance.LoadScannerTCP();
        }
        private void LoadModel()
        {
            BLLManager.Instance.LoadModel();
        }
        private void LoadTimeRunMachine()
        {
            BLLManager.Instance.LoadTimeRunMachine();
        }
        private async Task LoadMES()
        {
            await BLLManager.Instance.LoadMES();
        }
    }
}
