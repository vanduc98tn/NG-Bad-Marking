using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TimeInfor
    {
        public TimeSpan TotalTime { get; set; }
        public TimeSpan NormalRunTime { get; set; }
        public TimeSpan StopTime { get; set; }
        public TimeSpan AlarmTime { get; set; }
        public TimeInfor()
        {
            TotalTime = TimeSpan.Zero;
            NormalRunTime = TimeSpan.Zero;
            StopTime = TimeSpan.Zero;
            AlarmTime = TimeSpan.Zero;
        }
    }
}
