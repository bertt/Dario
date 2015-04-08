using Dario.Core.Convertors;
using Dario.Core.Esri;
using Dario.Models;

namespace Dario.DataReaders
{
    public class FeatureCollectionDataReader
    {
        public static FeatureCollection GetFeatureCollection(dynamic serverConfig, FeatureLayer featureLayerConfig)
        {
            FeatureCollection featureCollection = null;
            var datasourceType = (DatasourceTypes)serverConfig.type;

            switch (datasourceType)
            {
                case DatasourceTypes.Geojson:
                    var file = (string)serverConfig.file;
                    var geojson = GeojsonDataReader.ReadGeojsonFromFile(file);
                    featureCollection = geojson.ToEsriJJson();

                    // add some more
                    featureCollection.geometryType = featureLayerConfig.geometryType;
                    featureCollection.objectIdFieldName = featureLayerConfig.objectIdField;
                    featureCollection.spatialReference = new SpatialReference { wkid = (int)serverConfig.srid, latestWkid = (int)serverConfig.srid };
                    break;
                case DatasourceTypes.Postgis:
                    var dsn = (string)serverConfig.dsn;
                    var sql = (string)serverConfig.sql;
                    PostgisDataReader.ReadPostgis(dsn, sql);
                    break;
            }
            return featureCollection;
        }

    }
}