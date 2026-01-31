using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ITCPClient
    {
        Task<bool> Open();
        void Close();
        Task Send(String dataSend);
        void Receiver();
    }
}
