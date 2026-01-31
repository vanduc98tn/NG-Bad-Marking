using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public interface IObserverCOM : IObserver
    {
        void UpdateResultToUI(string name, string qr);
        void CheckConnectChange(string name, bool connected);
    }
}
