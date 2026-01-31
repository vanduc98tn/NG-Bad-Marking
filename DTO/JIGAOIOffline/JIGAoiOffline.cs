using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class JIGAoiOffline
    {
        public List<int> positionNGs { get; set; }
        public int pattern { get; set; }
        public JIGAoiOffline()
        {
            this.positionNGs = new List<int>();
            this.pattern = 1;
        }
    }
}
