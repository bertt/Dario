using Dario.Core.Esri;

namespace Dario.Core.Convertors
{
    public static class GeoJsonExtensions
    {
        public static FeatureCollection ToEsriJJson(this GeoJson.FeatureCollection geojsonFeatureCollection)
        {
            var esriFeatureCollection = new FeatureCollection();
            if (geojsonFeatureCollection.features.Count > 0)
            {
                foreach (var geosjonFeature in geojsonFeatureCollection.features)
                {
                    esriFeatureCollection.features.Add(geosjonFeature.ToEsriJson());
                }

                esriFeatureCollection.geometryType = "esriGeometryPolygon";
                esriFeatureCollection.spatialReference=new SpatialReference(){latestWkid = 4326,wkid=4326};
                //esriFeatureCollection.objectIdFieldName = clientConfig.objectIdField;
            }
            return esriFeatureCollection;
        }

        public static Feature ToEsriJson(this GeoJson.Feature geojsonFeature)
        {
            var esriFeature = new Feature();

            switch (geojsonFeature.geometry.type)
            {
                case "Polygon":
                case "MultiPolygon":
                    esriFeature.geometry.rings = geojsonFeature.geometry.coordinates;
                    break;

            }
            esriFeature.attributes = geojsonFeature.Properties;
            esriFeature.attributes.Add("objectid",geojsonFeature.id);
            return esriFeature;
        }
    }
}
