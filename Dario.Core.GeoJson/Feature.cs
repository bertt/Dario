namespace Dario.Core.GeoJson
{
    public class Feature
    {
        public string type { get; set; }
        public string id { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }
}
