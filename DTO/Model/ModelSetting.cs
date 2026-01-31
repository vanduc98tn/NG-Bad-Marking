using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ModelSetting
    {
        public const String DEFAULT_MODEL_NAME = "default";
        public string ModelName {  get; set; }
        public DateTime UpdatedTime { get; set; }
        public AutoTeching Teaching { get; set; }
        public ParaPatern Pattern { get; set; }
        public ModelSetting() 
        {
            this.UpdatedTime = DateTime.Now;
            this.ModelName = DEFAULT_MODEL_NAME;
            this.Teaching = new AutoTeching();
            this.Pattern = new ParaPatern();
        }
        public Boolean HasSameModel(ModelSetting x)
        {
            if (this.ModelName != null && x != null && this.ModelName.Equals(x.ModelName))
            {
                return true;
            }
            return false;
        }
        public String ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static ModelSetting FromJSON(String js)
        {
            var j = JsonConvert.DeserializeObject<ModelSetting>(js);
            if (j.Teaching == null)
            {
                j.Teaching = new AutoTeching();
            }
            if(j.Pattern == null)
            {
                j.Pattern = new ParaPatern();
            }
            return j;
        }
    }
}
