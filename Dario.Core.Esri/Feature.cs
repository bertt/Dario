using System.Collections.Generic;
using System.Dynamic;

namespace Dario.Core.Esri
{
    public class Feature
    {
        public Feature()
        {
            geometry = new ExpandoObject();
        }
        public dynamic geometry { get; set; }
        public Dictionary<string, object> attributes { get; set; }
    }
}
