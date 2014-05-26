using System.Collections.Generic;

namespace Dario.Core.GeoJson
{
    public class FeatureCollection
    {
        public FeatureCollection()
        {
            features=new List<Feature>();
        }
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }
}

