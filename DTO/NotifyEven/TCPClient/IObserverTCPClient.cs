using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public interface IObserverTCPClient:IObserver
    {
        void UpdateResultToUI(string name,string notify);
        void CheckConnectChange(string name,bool connected);
    }
}
