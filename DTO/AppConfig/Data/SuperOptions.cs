using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SuperOptions
    {
        public bool DoorUse { get; set; }
        public string BinCode_OK {  get; set; }
        public string BinCode_NG { get; set; }
        public SuperOptions()
        {
            this.DoorUse = false;
            this.BinCode_OK = "0";
            this.BinCode_NG = "1";
        }
    }
}
