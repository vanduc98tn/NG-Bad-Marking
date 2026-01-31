using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NotifyEvenTCPClient
    {
        private List<IObserverTCPClient> observers = new List<IObserverTCPClient>();
        public List<IObserverTCPClient> Observers => observers;
        public void Attach(IObserverTCPClient observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserverTCPClient observer)
        {
            observers.Remove(observer);
        }
        public void NotifyToUI(string notify)
        {
            foreach (IObserverTCPClient ob in observers)
            {
                ob.UpdateNotifyToUI(notify);
            }
        }
        public void NotifyResultUI(string name, string notify)
        {
            foreach (IObserverTCPClient ob in observers)
            {
                ob.UpdateResultToUI(name, notify);
            }
        }
        public void NotifyConnectionChange(string name,bool connection)
        {
            foreach (IObserverTCPClient ob in observers)
            {
                ob.CheckConnectChange(name, connection);
            }
        }
    }
}
