using System.Collections.Generic;
using System.Dynamic;

namespace Dario.Core.Esri
{
    public class Feature
    {
        public Feature()
        {
            Geometry = new ExpandoObject();
        }
        public dynamic Geometry { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}
