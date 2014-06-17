using Newtonsoft.Json;

namespace Dario.Core.Esri
{
    public abstract class Geometry
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SpatialReference spatialReference { get; set; }

        //add methods to convert the geometry to/from WKT
        // abstract public string ToWKT();
    }
}
