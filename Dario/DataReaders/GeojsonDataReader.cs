using System.IO;
using Dario.Core.GeoJson;
using Newtonsoft.Json;

namespace Dario.DataReaders
{
    public static class GeojsonDataReader
    {
        public static FeatureCollection ReadGeojsonFromFile(string file)
        {
            var content = File.ReadAllText(file);
            var featureCollection = ReadGeojson(content);

            return featureCollection;
        }

        public static FeatureCollection ReadGeojson(string content)
        {
            var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(content);
            return featureCollection;
        }
    }
}