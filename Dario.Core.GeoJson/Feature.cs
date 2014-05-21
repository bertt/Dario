using System.Collections.Generic;

namespace Dario.Core.GeoJson
{
    public class Feature
    {
        public string type { get; set; }
        public string id { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public Geometry geometry { get; set; }
        public Style style { get; set; }
    }
}
