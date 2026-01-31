using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RunSetting
    {
        public bool MESOnline { get; set; }
        public bool AOIOnline { get; set; }

        public bool UseE011 { get; set; }
        public RunSetting()
        {
            this.MESOnline = false;
            this.AOIOnline = false;
            this.UseE011 = false;
        }
    }
}
