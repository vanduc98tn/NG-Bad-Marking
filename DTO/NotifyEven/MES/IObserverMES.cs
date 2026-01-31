using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public interface IObserverMES: IObserver
    {
        void CheckConnectionMES(bool connected);
        void FollowingDataMES(string MESResult);
        void GetInformationFromClientConnect(string clientIP,int clientPort);
    }
}
