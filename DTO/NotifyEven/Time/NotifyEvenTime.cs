using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NotifyEvenTime
    {
        private List<IObserverTime> observers = new List<IObserverTime>();
        public List<IObserverTime> Observers => observers;
        public void Attach(IObserverTime observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserverTime observer)
        {
            observers.Remove(observer);
        }
        public void NotifyTimeTotal(string timeNotify)
        {
            foreach (IObserverTime ob in observers)
            {
                ob.UpdateTimeTotal(timeNotify);
            }
        }
        public void NotifyTimeNormal(string timeNotify)
        {
            foreach (IObserverTime ob in observers)
            {
                ob.UpdateTimeNormal(timeNotify);
            }
        }
        public void NotifyTimeStop(string timeNotify)
        {
            foreach (IObserverTime ob in observers)
            {
                ob.UpdateTimeStop(timeNotify);
            }
        }
        public void NotifyTimeAlarm(string timeNotify)
        {
            foreach (IObserverTime ob in observers)
            {
                ob.UpdateTimeAlarm(timeNotify);
            }
        }
    }
}
