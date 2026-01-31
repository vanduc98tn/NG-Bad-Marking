using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class ServiceModel
    {
        private LoggerDebug logger = new LoggerDebug("ServiceModel");
        private ModelStore modelStore;
        public ServiceModel()
        {
            this.modelStore = new ModelStore();
        }
        public ModelSetting GetModelSettings(string modelName)
        {
            return this.modelStore.GetModelSettings(modelName);
        }
        public void UpdateModelSettings(ModelSetting newSettings)
        {
            this.modelStore.UpdateModelSettings(newSettings);
        }
        public List<ModelInfo> GetModelInfoList()
        {
            return this.modelStore.GetModelInfoList();
        }
        public void DeleteModel(String model)
        {
            this.modelStore.DeleteModel(model);
        }
        public void DeleteAll()
        { this.modelStore.DeleteAll(); }
    }
}
