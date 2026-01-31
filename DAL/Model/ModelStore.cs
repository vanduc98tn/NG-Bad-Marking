using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Newtonsoft;
using Newtonsoft.Json;

namespace DAL
{
    public class ModelStore
    {
        private static String MODEL_SETTINGS_FILE_NAME = "model_settings.json";
        private static LoggerDebug logger = new LoggerDebug("ModelStore");
        private static Object lockObj = new object();

        public ModelSetting GetModelSettings(string modelName)
        {
            ModelSetting ret = null;
            lock (lockObj)
            {
                try
                {
                    // Load settings from file:
                    String filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), MODEL_SETTINGS_FILE_NAME);
                    if (File.Exists(filePath))
                    {
                        using (StreamReader file = File.OpenText(filePath))
                        {
                            var js = file.ReadToEnd();
                            var specList = JsonConvert.DeserializeObject<ModelSetting[]>(js);
                            if (specList == null) return null;
                            foreach (var x in specList)
                            {
                                if (x.ModelName.Equals(modelName))
                                {
                                    ret = x;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Create("GetModelSettings:" + ex.Message,LogLevel.Error);
                }
            }
            return ret;
        }
        public void UpdateModelSettings(ModelSetting newSettings)
        {
            lock (lockObj)
            {
                try
                {
                    List<ModelSetting> modelList = new List<ModelSetting>(0);

                    String filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), MODEL_SETTINGS_FILE_NAME);
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            using (StreamReader file = File.OpenText(filePath))
                            {
                                var js = file.ReadToEnd();
                                modelList.AddRange(JsonConvert.DeserializeObject<ModelSetting[]>(js));
                            }
                        }
                        catch (Exception ex1)
                        {
                            logger.Create("Load JSON from file:" + ex1.Message,LogLevel.Error);
                        }
                    }

                    // Update existing:
                    var hasUpdated = false;
                    for (int i = 0; i < modelList.Count; i++)
                    {
                        if (modelList[i].HasSameModel(newSettings))
                        {
                            modelList[i] = newSettings;
                            hasUpdated = true;
                            break;
                        }
                    }

                    // Add new:
                    if (!hasUpdated)
                    {
                        modelList.Add(newSettings);
                    }

                    // Store:
                    var jsNew = JsonConvert.SerializeObject(modelList);
                    File.WriteAllText(filePath, jsNew);
                }
                catch (Exception ex)
                {
                    logger.Create("UpdateModelSettings: " + ex.Message,LogLevel.Error);
                }
            }
        }
        public List<ModelInfo> GetModelInfoList()
        {
            List<ModelInfo> ret = new List<ModelInfo>();

            lock (lockObj)
            {
                try
                {
                    ModelInfo.ResetIndex();

                    String filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), MODEL_SETTINGS_FILE_NAME);
                    if (File.Exists(filePath))
                    {
                        using (StreamReader file = File.OpenText(filePath))
                        {
                            var js = file.ReadToEnd();
                            var models = JsonConvert.DeserializeObject<ModelSetting[]>(js);
                            foreach (var x in models)
                            {
                                var modelInfo = new ModelInfo(x.ModelName, x.UpdatedTime);
                                ret.Add(modelInfo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Create("GetModelInfoList: " + ex.Message,LogLevel.Error);
                }
            }
            return ret;
        }
        public void DeleteModel(String model)
        {
            lock (lockObj)
            {
                try
                {
                    List<ModelSetting> modelList = new List<ModelSetting>(0);

                    String filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), MODEL_SETTINGS_FILE_NAME);
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            using (StreamReader file = File.OpenText(filePath))
                            {
                                var js = file.ReadToEnd();
                                modelList.AddRange(JsonConvert.DeserializeObject<ModelSetting[]>(js));
                            }
                        }
                        catch (Exception ex1)
                        {
                            logger.Create("Load JSON from file error:" + ex1.Message, LogLevel.Error);
                        }
                    }

                    // Find & delete model:
                    var newList = new List<ModelSetting>();
                    var hasDelete = false;
                    for (int i = 0; i < modelList.Count; i++)
                    {
                        if (!modelList[i].ModelName.Equals(model))
                        {
                            newList.Add(modelList[i]);
                        }
                        else
                        {
                            hasDelete = true;
                        }
                    }

                    // Store:
                    if (hasDelete)
                    {
                        var jsNew = JsonConvert.SerializeObject(newList);
                        File.WriteAllText(filePath, jsNew);
                    }
                }
                catch (Exception ex)
                {
                    logger.Create("DeleteModel: " + ex.Message, LogLevel.Error);
                }
            }
        }
        public void DeleteAll()
        {
            lock (lockObj)
            {
                try
                {
                    List<ModelSetting> modelList = new List<ModelSetting>(0);
                    String filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), MODEL_SETTINGS_FILE_NAME);

                    // Store:
                    var jsNew = JsonConvert.SerializeObject(modelList);
                    File.WriteAllText(filePath, jsNew);

                }
                catch (Exception ex)
                {
                    logger.Create("DeleteAll: " + ex.Message,LogLevel.Error);
                }
            }
        }
    }
    
}
