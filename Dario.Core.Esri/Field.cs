using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dario.Core.Esri
{
    public class Field
    {
        public string name { get; set; }
        public string alias { get; set; }
        public bool required { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public esriFieldType type { get; set; }
        public int length { get; set; }
        public Domain domain { get; set; }
        public bool editable { get; set; }

    }
}
