using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NotifyEvenCOM
    {
        private List<IObserverCOM> observers = new List<IObserverCOM>();
        public List<IObserverCOM> Observers => observers;
        public void Attach(IObserverCOM observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserverCOM observer)
        {
            observers.Remove(observer);
        }
        public void NotifyToUI(string notify)
        {
            foreach (IObserverCOM ob in observers)
            {
                ob.UpdateNotifyToUI(notify);
            }
        }
        public void NotifyResultUI(string name,string notify)
        {
            foreach (IObserverCOM ob in observers)
            {
                ob.UpdateResultToUI(name, notify);
            }
        }
        public void CheckConnectChange(string name, bool connection)
        {
            foreach (IObserverCOM ob in observers)
            {
                ob.CheckConnectChange(name, connection);
            }
        }
    }
}
