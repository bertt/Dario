using System.Collections.Generic;

namespace Dario.Core.Esri
{
    public class Feature
    {
        public dynamic geometry { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}
