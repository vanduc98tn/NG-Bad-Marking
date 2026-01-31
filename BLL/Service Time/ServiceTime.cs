using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class ServiceTime
    {
        private TimeMachine timeMachine;
        public ServiceTime() {
            this.timeMachine = new TimeMachine();
        }
        public void StartTotalTime()
        { 
            this.timeMachine.StartTotalTime();
        }
        public void StopTotalTime() {  
            this.timeMachine.StopTotalTime();
        }
        public void StartNormalTime() { 
            this.timeMachine.StartNormalTime();
        }
        public void StopNormalTime() { 
            this.timeMachine.StopNormalTime();
        }
        public void StartPauseTime() {  
            this.timeMachine.StartPauseTime();
        }
        public void StopPauseTime() { 
            this.timeMachine.StopPauseTime();
        }
        public void StartAlarmTime() {  
            this.timeMachine.StartAlarmTime();}
        public void StopAlarmTime() { 
            this.timeMachine.StopAlarmTime();}
    }
}
