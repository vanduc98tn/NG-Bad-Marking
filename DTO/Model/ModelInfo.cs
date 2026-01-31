using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ModelInfo
    {
        private static int _index = 0;

        public int Index { get; set; }
        public String Name { get; set; }
        public String Time { get; set; }

        public static void ResetIndex()
        {
            _index = 0;
        }

        public ModelInfo(String name, DateTime updatedTime)
        {
            _index++;
            this.Index = _index;
            this.Name = name;
            this.Time = updatedTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
