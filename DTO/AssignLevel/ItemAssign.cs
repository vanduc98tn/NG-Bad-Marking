using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ItemAssign
    {
        public List<Item> lstAssignOperater { get; set; }
        public List<Item> lstAssignManager { get; set; }
        public ItemAssign()
        {
            this.lstAssignOperater = new List<Item>();
            this.lstAssignManager = new List<Item>();
        }
    }
    public class Item
    {
        public string AssignName { get; set; }
        public bool Isview { get; set; }
    }
}
