using System.Collections.Generic;

namespace Dario.Core.Esri
{
    public class FeatureLayer
    {
        public float currentVersion { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string displayField { get; set; }
        public string objectIdField { get; set; }
        public string description { get; set; }
        public string copyrightText { get; set; }
        public bool defaultVisibility { get; set; }
        public bool isDataVersioned { get; set; }
        public bool supportsRollbackOnFailureParameter { get; set; }
        public bool supportsAdvancedQueries { get; set; }
        public string geometryType { get; set; }
        public int minScale { get; set; }
        public int maxScale { get; set; }
        public bool hasStaticData { get; set; }
        public int maxRecordCount { get; set; }
        public string capabilities { get; set; }
        public string supportedQueryFormats { get; set; }
        public Extent extent { get; set; }
        public List<Field> fields { get; set; }
    }
}
