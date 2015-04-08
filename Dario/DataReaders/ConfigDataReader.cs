using System.IO;
using Dario.Core.Esri;
using Newtonsoft.Json;

namespace Dario.DataReaders
{
    public static class ConfigDataReader
    {
        public static FeatureLayer GetFeatureLayerConfig(string configDir, string serviceName, string layerId)
        {
            var path = configDir + @"FeatureServer\" + serviceName + @"\" + serviceName + "_" + layerId + ".json";

            if (File.Exists(path))
            {
                var jsonObject = ReadObject(path);
                return jsonObject;
            }
            return null;
        }


        public static FeatureLayer ReadObject(string path)
        {
            var json = File.ReadAllText(path);
            var jsonObject = JsonConvert.DeserializeObject<FeatureLayer>(json);
            return jsonObject;
        }
    }
}