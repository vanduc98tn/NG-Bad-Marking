using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public interface IObserverTime
    {
        void UpdateTimeTotal(string time);
        void UpdateTimeNormal(string time);
        void UpdateTimeStop(string time);
        void UpdateTimeAlarm(string time);
    }
}
