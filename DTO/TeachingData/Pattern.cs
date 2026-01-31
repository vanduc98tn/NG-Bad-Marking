using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Pattern
    {
        public PositionTeaching pos { get; set; }
        public Pattern()
        {
            pos = new PositionTeaching();
        }
    }
}
