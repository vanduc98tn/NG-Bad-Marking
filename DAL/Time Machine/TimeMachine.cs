using DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DTO;
using System.Timers;

namespace DAL
{
    public class TimeMachine
    {
        private static System.Timers.Timer timer = new System.Timers.Timer(1000);
        private static volatile bool isAutoRunning = false;
        private static volatile bool isAlarming = false;
        public static bool IsStartTime = false;
        public static bool IsStopTime = false;
        public static bool IsAlarmTime = false;
        private CancellationTokenSource totalTimeCancellation;
        Stopwatch stopwatchTotalTime;
        private CancellationTokenSource normalTimeCancellation;
        Stopwatch stopwatchnormalTime;
        private CancellationTokenSource stopTimeCancellation;
        Stopwatch stopwatchstopTime;
        private CancellationTokenSource alarmTimeCancellation;
        Stopwatch stopwatchalarmTime;
        public TimeMachine()
        {
            totalTimeCancellation = new CancellationTokenSource();
            normalTimeCancellation = new CancellationTokenSource();
            stopTimeCancellation = new CancellationTokenSource();
            alarmTimeCancellation = new CancellationTokenSource();

            string timetotal = this.FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.TotalTime);
            SystemsManager.Instance.NotifyEvenTime.NotifyTimeTotal(timetotal);

            string timenormal = this.FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.NormalRunTime);
            SystemsManager.Instance.NotifyEvenTime.NotifyTimeTotal(timenormal);

            string timestop = this.FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.StopTime);
            SystemsManager.Instance.NotifyEvenTime.NotifyTimeTotal(timestop);

            string timeAlarm = this.FormatElapsedTime(SystemsManager.Instance.AppSettings.TimeInfor.AlarmTime);
            SystemsManager.Instance.NotifyEvenTime.NotifyTimeTotal(timeAlarm);
        }

        public async void StartTotalTime()
        {
            TimeSpan offset = SystemsManager.Instance.AppSettings.TimeInfor.TotalTime;
            totalTimeCancellation = new CancellationTokenSource();
            stopwatchTotalTime = new Stopwatch();
            stopwatchTotalTime.Start();

            // Vòng lặp
            while (!totalTimeCancellation.Token.IsCancellationRequested)
            {
                // Tạm dừng vòng lặp 1 giây
                await Task.Delay(1000);

                // Lấy thời gian đã đo được và cộng thêm khoảng thời gian chênh lệch
                TimeSpan elapsedTime = stopwatchTotalTime.Elapsed + offset;
                SystemsManager.Instance.AppSettings.TimeInfor.TotalTime = elapsedTime;
                string time = this.FormatElapsedTime(elapsedTime);
                // Hiển thị thời gian đã đo được
                SystemsManager.Instance.NotifyEvenTime.NotifyTimeTotal(time);
            }
        }
        public void StopTotalTime()
        {
            totalTimeCancellation?.Cancel();
            stopwatchTotalTime?.Stop();
            SystemsManager.Instance.SaveAppSettings();

        }
        public async void StartNormalTime()
        {
            if (IsStartTime) return;
            IsStartTime = true;
            TimeSpan offset = SystemsManager.Instance.AppSettings.TimeInfor.NormalRunTime;
            normalTimeCancellation = new CancellationTokenSource();
            stopwatchnormalTime = new Stopwatch();
            stopwatchnormalTime.Start();

            // Vòng lặp
            while (!normalTimeCancellation.Token.IsCancellationRequested)
            {
                // Tạm dừng vòng lặp 1 giây
                await Task.Delay(1000);

                // Lấy thời gian đã đo được và cộng thêm khoảng thời gian chênh lệch
                TimeSpan elapsedTime = stopwatchnormalTime.Elapsed + offset;
                SystemsManager.Instance.AppSettings.TimeInfor.NormalRunTime = elapsedTime;
                string time = this.FormatElapsedTime(elapsedTime);
                // Hiển thị thời gian đã đo được
                SystemsManager.Instance.NotifyEvenTime.NotifyTimeNormal(time);
            }
        }
        public void StopNormalTime()
        {
            IsStartTime = false;
            normalTimeCancellation?.Cancel();
            stopwatchnormalTime?.Stop();
            SystemsManager.Instance.SaveAppSettings();
        }
        public async void StartPauseTime()
        {
            if (IsStopTime) return;
            IsStopTime = true;
            TimeSpan offset = SystemsManager.Instance.AppSettings.TimeInfor.StopTime;
            stopTimeCancellation = new CancellationTokenSource();
            stopwatchstopTime = new Stopwatch();
            stopwatchstopTime.Start();

            // Vòng lặp
            while (!stopTimeCancellation.Token.IsCancellationRequested)
            {
                // Tạm dừng vòng lặp 1 giây
                await Task.Delay(1000);

                // Lấy thời gian đã đo được và cộng thêm khoảng thời gian chênh lệch
                TimeSpan elapsedTime = stopwatchstopTime.Elapsed + offset;
                SystemsManager.Instance.AppSettings.TimeInfor.StopTime = elapsedTime;
                string time = this.FormatElapsedTime(elapsedTime);
                // Hiển thị thời gian đã đo được
                SystemsManager.Instance.NotifyEvenTime.NotifyTimeStop(time);
            }
        }
        public void StopPauseTime()
        {
            IsStopTime = false;
            stopTimeCancellation?.Cancel();
            stopwatchstopTime?.Stop();
            SystemsManager.Instance.SaveAppSettings();
        }
        public async void StartAlarmTime()
        {
            if (IsAlarmTime) return;
            IsAlarmTime = true;
            TimeSpan offset = SystemsManager.Instance.AppSettings.TimeInfor.AlarmTime;
            alarmTimeCancellation = new CancellationTokenSource();
            stopwatchalarmTime = new Stopwatch();
            stopwatchalarmTime.Start();

            // Vòng lặp
            while (!alarmTimeCancellation.Token.IsCancellationRequested)
            {
                // Tạm dừng vòng lặp 1 giây
                await Task.Delay(1000);

                // Lấy thời gian đã đo được và cộng thêm khoảng thời gian chênh lệch
                TimeSpan elapsedTime = stopwatchalarmTime.Elapsed + offset;
                SystemsManager.Instance.AppSettings.TimeInfor.AlarmTime = elapsedTime;
                string time = this.FormatElapsedTime(elapsedTime);
                // Hiển thị thời gian đã đo được
                SystemsManager.Instance.NotifyEvenTime.NotifyTimeAlarm(time);
            }

        }
        public void StopAlarmTime()
        {
            IsAlarmTime = false;
            alarmTimeCancellation?.Cancel();
            stopwatchalarmTime?.Stop();
            SystemsManager.Instance.SaveAppSettings();
        }
        private string FormatElapsedTime(TimeSpan elapsed)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }
    }
}
