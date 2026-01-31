using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ParaPatern
    {
        public int CurrentPatern { get; set; }
        public double PitchX { get; set; }
        public double PitchY { get; set; }
        public int xRow { get; set; }
        public int yColumn { get; set; }
        public double offsetX { get; set; }
        public double offsetY { get; set; }
        public bool Use2Matrix { get; set; }
        public ParaPatern() 
        {
            this.CurrentPatern = 0;
            this.PitchX= 0;
            this.PitchY= 0;
            this.xRow = 0;
            this.yColumn = 0;
            this.offsetX = 0;
            this.offsetY = 0;
            this.Use2Matrix = false;
        }

    }
}
