using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dario.Core.Convertors;
using Dario.Core.GeoJson;
using Dario.Models;
using Newtonsoft.Json;
using Npgsql;
using Dapper;

namespace Dario.Controllers
{
    public class FeatureServerController:ApiController
    {

        // http://localhost:49430/rest/services/Treinen/FeatureServer
        [Route("rest/services/{serviceName}/FeatureServer")]
        public HttpResponseMessage GetFeatureServer(string serviceName)
        {
            var agsConfigDir = ConfigurationManager.AppSettings["AgsConfigDir"];
            var path = agsConfigDir + @"FeatureServer\" + serviceName + @"\" + serviceName + ".json";

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var jsonObject = JsonConvert.DeserializeObject(json);
                return Request.CreateResponse(HttpStatusCode.OK,jsonObject);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // http://localhost:49430/rest/services/Treinen/FeatureServer/0
        [Route("rest/services/{serviceName}/FeatureServer/{layerId}")]
        public HttpResponseMessage GetFeatureServer(string serviceName,string layerId)
        {
            var agsConfigDir = ConfigurationManager.AppSettings["AgsConfigDir"];
            var path = agsConfigDir + @"FeatureServer\" + serviceName + @"\" + serviceName + "_"+ layerId + ".json";

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var jsonObject = JsonConvert.DeserializeObject(json);
                return Request.CreateResponse(HttpStatusCode.OK, jsonObject);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }


        // sample 1 http://localhost:49430/rest/services/Countries/FeatureServer/0/query?f=json&returnIdsOnly=true&returnCountOnly=true&where=1%3D1&returnGeometry=false&spatialRel=esriSpatialRelIntersects&outFields=*&outSR=28992&callback=dojo.io.script.jsonp_dojoIoScript13._jsonpCallback
        // sample 2: http://localhost:49430/rest/services/treinen/FeatureServer/0/query/query?returnGeometry=true&spatialRel=esriSpatialRelIntersects&where=1%3d1&outSR=102100&maxAllowableOffset=38.2185141425367&outFields=*&orderByFields=ID+ASC&f=json
        [Route("rest/services/{serviceName}/FeatureServer/{layerId}/query")]
        [Route("rest/services/{serviceName}/FeatureServer/{layerId}/query/{operation}")]
        public HttpResponseMessage GetQuery(string serviceName, string layerId, string operation = "",
            bool returnGeometry=true,
            string f = "json",
            string objectids = "",
            string where = "",
            string time = "",
            string geometry = "",
            string geometryType = "",
            string geometryPrecision = "",
            string inSr = "",
            string spatialRel = "",
            string outSr = "",
            string returnIdsOnly = "",
            string returnCountOnly = "",
            string maxAllowableOffset = "",
            string returnDistinctValues = "",
            string groupByFieldsForStatistics = "",
            string outStatistics = "",
            string outFields = "",
            string orderByFields="",
            string objectIds ="",
            bool returnZ = false,
            bool returnM= false
            )
        {
            // open settings for service, layer
            Core.Esri.FeatureCollection result = null;

            // first let's read the server config file
            var agsConfigDir = ConfigurationManager.AppSettings["AgsConfigDir"];
            var serverconfig = agsConfigDir + @"FeatureServer\" + serviceName + @"\"+ serviceName + "_" + layerId + "_server.json";
            if (File.Exists(serverconfig))
            {
                var content = File.ReadAllText(serverconfig);
                dynamic o = JsonConvert.DeserializeObject(content);
                var datasourceType = (DatasourceTypes) o.type;

                switch (datasourceType)
                {
                    case DatasourceTypes.Geojson:
                        var file = (string)o.file;
                        result = ReadGeojson(file);
                        break;
                    case DatasourceTypes.Postgis:
                        var dsn = (string)o.dsn;
                        var sql = (string)o.sql;
                        ReadPostgis(dsn, sql);
                        break;
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, result); 
        }

        private Core.Esri.FeatureCollection ReadGeojson(string file)
        {
            var content = File.ReadAllText(file);
            var featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(content);

            // convert to esri format and return 
            return featureCollection.ToEsriJJson();
        }

        private void ReadPostgis(string connectionString, string sql)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var res = conn.Query(sql);
                foreach (var p in res)
                {
                    // todo: create something here

                }
            }
        }


    }
}


/**
 * {
  "objectIdFieldName": "ID",
  "globalIdFieldName": "",
  "geometryType": "esriGeometryPoint",
  "spatialReference": {
    "wkid": 102100,
    "latestWkid": 102100
  },
  "fields": [
    {
      "name": "ID",
      "alias": "ID",
      "required": false,
      "type": "esriFieldTypeInteger",
      "length": 0,
      "editable": false
    },
    {
      "name": "Name",
      "alias": "Name",
      "required": false,
      "type": "esriFieldTypeString",
      "length": 0,
      "editable": false
    },
    {
      "name": "Date",
      "alias": "date",
      "required": false,
      "type": "esriFieldTypeDate",
      "length": 0,
      "editable": false
    },
    {
      "name": "Speed",
      "alias": "Speed",
      "required": false,
      "type": "esriFieldTypeDouble",
      "length": 0,
      "editable": false
    }
  ],
  "features": [
    {
      "geometry": {
        "x": 566017.4,
        "y": 6919094.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613050,
        "ID": 230613050,
        "Name": "2143",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 677106.9,
        "y": 6892273.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613164,
        "ID": 230613164,
        "Name": "463",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 550749.9,
        "y": 6864490.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613169,
        "ID": 230613169,
        "Name": "2969",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 676390.563,
        "y": 6890666.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613071,
        "ID": 230613071,
        "Name": "482",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 641316.8,
        "y": 6703187.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613072,
        "ID": 230613072,
        "Name": "964",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 126.0
      }
    },
    {
      "geometry": {
        "x": 620991.938,
        "y": 6792621.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613085,
        "ID": 230613085,
        "Name": "2442",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 766861.5,
        "y": 6840389.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613086,
        "ID": 230613086,
        "Name": "3422",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 574196.3,
        "y": 6863864.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613087,
        "ID": 230613087,
        "Name": "2430",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 108.0
      }
    },
    {
      "geometry": {
        "x": 561723.1,
        "y": 6721996.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613089,
        "ID": 230613089,
        "Name": "2464",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 64.0
      }
    },
    {
      "geometry": {
        "x": 567443.563,
        "y": 6853225.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613090,
        "ID": 230613090,
        "Name": "2408",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 122.0
      }
    },
    {
      "geometry": {
        "x": 795755.438,
        "y": 6856130.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613091,
        "ID": 230613091,
        "Name": "1745",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 496076.2,
        "y": 6786404.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613092,
        "ID": 230613092,
        "Name": "2409",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 589327.8,
        "y": 6744562.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613093,
        "ID": 230613093,
        "Name": "965",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 714019.25,
        "y": 6851426.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613094,
        "ID": 230613094,
        "Name": "2970",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 79.0
      }
    },
    {
      "geometry": {
        "x": 656776.25,
        "y": 6797431.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613095,
        "ID": 230613095,
        "Name": "2961",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 550321.063,
        "y": 6864605.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613096,
        "ID": 230613096,
        "Name": "7046",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 731417.563,
        "y": 6865673.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613097,
        "ID": 230613097,
        "Name": "3426",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 75.0
      }
    },
    {
      "geometry": {
        "x": 483080.219,
        "y": 6813242.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613098,
        "ID": 230613098,
        "Name": "1746",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 611183.6,
        "y": 6700284.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613099,
        "ID": 230613099,
        "Name": "951",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 487984.0,
        "y": 6813484.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613100,
        "ID": 230613100,
        "Name": "2962",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 766858.6,
        "y": 6840418.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613101,
        "ID": 230613101,
        "Name": "2983",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 545549.75,
        "y": 6868979.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613102,
        "ID": 230613102,
        "Name": "2460",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488217.656,
        "y": 6813445.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613103,
        "ID": 230613103,
        "Name": "2115",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 523058.9,
        "y": 6853296.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613104,
        "ID": 230613104,
        "Name": "2461",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 573970.75,
        "y": 6851522.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613105,
        "ID": 230613105,
        "Name": "2411",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 36.0
      }
    },
    {
      "geometry": {
        "x": 611892.438,
        "y": 6700336.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613120,
        "ID": 230613120,
        "Name": "949",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 496412.531,
        "y": 6786271.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613121,
        "ID": 230613121,
        "Name": "2958",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 566567.3,
        "y": 6817861.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613122,
        "ID": 230613122,
        "Name": "2647",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 652065.5,
        "y": 6769976.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613162,
        "ID": 230613162,
        "Name": "2956",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 589666.438,
        "y": 6745603.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613054,
        "ID": 230613054,
        "Name": "453",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 611256.6,
        "y": 6700200.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613058,
        "ID": 230613058,
        "Name": "444",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 629559.3,
        "y": 6669503.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613059,
        "ID": 230613059,
        "Name": "939",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 136.0
      }
    },
    {
      "geometry": {
        "x": 634906.063,
        "y": 6596077.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613060,
        "ID": 230613060,
        "Name": "922",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 459502.5,
        "y": 6795797.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613061,
        "ID": 230613061,
        "Name": "926",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 481609.2,
        "y": 6814736.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613062,
        "ID": 230613062,
        "Name": "2628",
        "DateGps": "2014-05-15T11:33:31.043Z",
        "Speed": 7.0
      }
    },
    {
      "geometry": {
        "x": 609898.2,
        "y": 6699961.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613063,
        "ID": 230613063,
        "Name": "947",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 43.0
      }
    },
    {
      "geometry": {
        "x": 516963.969,
        "y": 6870385.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613064,
        "ID": 230613064,
        "Name": "2976",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 43.0
      }
    },
    {
      "geometry": {
        "x": 579942.563,
        "y": 6733011.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613065,
        "ID": 230613065,
        "Name": "2614",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 126.0
      }
    },
    {
      "geometry": {
        "x": 490887.4,
        "y": 6785976.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613066,
        "ID": 230613066,
        "Name": "458",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 14.0
      }
    },
    {
      "geometry": {
        "x": 532038.7,
        "y": 6727249.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613067,
        "ID": 230613067,
        "Name": "2659",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 634965.0,
        "y": 6596279.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613068,
        "ID": 230613068,
        "Name": "930",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 615542.0,
        "y": 6698741.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613069,
        "ID": 230613069,
        "Name": "955",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 122.0
      }
    },
    {
      "geometry": {
        "x": 611310.7,
        "y": 6700304.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613070,
        "ID": 230613070,
        "Name": "929",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 542179.2,
        "y": 6870731.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613073,
        "ID": 230613073,
        "Name": "2424",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 68.0
      }
    },
    {
      "geometry": {
        "x": 488195.469,
        "y": 6813513.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613074,
        "ID": 230613074,
        "Name": "2641",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 476854.9,
        "y": 6783865.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613075,
        "ID": 230613075,
        "Name": "465",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 111.0
      }
    },
    {
      "geometry": {
        "x": 623668.75,
        "y": 6703226.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613076,
        "ID": 230613076,
        "Name": "1761",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 126.0
      }
    },
    {
      "geometry": {
        "x": 614252.3,
        "y": 6805969.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613077,
        "ID": 230613077,
        "Name": "2444",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 61.0
      }
    },
    {
      "geometry": {
        "x": 603631.0,
        "y": 6780182.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613078,
        "ID": 230613078,
        "Name": "2637",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 574662.6,
        "y": 6848640.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613079,
        "ID": 230613079,
        "Name": "2957",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 57.0
      }
    },
    {
      "geometry": {
        "x": 678920.4,
        "y": 6891509.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613080,
        "ID": 230613080,
        "Name": "3424",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 566578.6,
        "y": 6817994.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613081,
        "ID": 230613081,
        "Name": "2617",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 485125.4,
        "y": 6815620.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613082,
        "ID": 230613082,
        "Name": "2417",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 79.0
      }
    },
    {
      "geometry": {
        "x": 573029.563,
        "y": 6820934.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613083,
        "ID": 230613083,
        "Name": "2113",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 584290.75,
        "y": 6825836.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613084,
        "ID": 230613084,
        "Name": "479",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 43.0
      }
    },
    {
      "geometry": {
        "x": 657367.4,
        "y": 6797406.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613106,
        "ID": 230613106,
        "Name": "3409",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 624473.25,
        "y": 6703553.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613107,
        "ID": 230613107,
        "Name": "2937",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 32.0
      }
    },
    {
      "geometry": {
        "x": 536468.5,
        "y": 6883134.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613108,
        "ID": 230613108,
        "Name": "2144",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 32.0
      }
    },
    {
      "geometry": {
        "x": 544331.6,
        "y": 6861316.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613109,
        "ID": 230613109,
        "Name": "2609",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 579378.5,
        "y": 6867841.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613110,
        "ID": 230613110,
        "Name": "2610",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 79.0
      }
    },
    {
      "geometry": {
        "x": 651382.25,
        "y": 6771695.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613111,
        "ID": 230613111,
        "Name": "1734",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 680385.0,
        "y": 6691029.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613112,
        "ID": 230613112,
        "Name": "1764",
        "DateGps": "2014-05-15T11:35:29.043Z",
        "Speed": 133.0
      }
    },
    {
      "geometry": {
        "x": 610197.9,
        "y": 6700003.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613113,
        "ID": 230613113,
        "Name": "1744",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 478454.8,
        "y": 6802826.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613114,
        "ID": 230613114,
        "Name": "2453",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 487981.156,
        "y": 6813587.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613115,
        "ID": 230613115,
        "Name": "2971",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 480879.625,
        "y": 6812441.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613116,
        "ID": 230613116,
        "Name": "2135",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 756613.063,
        "y": 6847526.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613117,
        "ID": 230613117,
        "Name": "3407",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 3.0
      }
    },
    {
      "geometry": {
        "x": 682062.2,
        "y": 6855862.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613118,
        "ID": 230613118,
        "Name": "1753",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 129.0
      }
    },
    {
      "geometry": {
        "x": 488052.9,
        "y": 6813521.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613119,
        "ID": 230613119,
        "Name": "2973",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 519650.281,
        "y": 6765368.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613123,
        "ID": 230613123,
        "Name": "2445",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 634832.0,
        "y": 6596140.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613124,
        "ID": 230613124,
        "Name": "1766",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 483322.531,
        "y": 6813319.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613125,
        "ID": 230613125,
        "Name": "2941",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 496133.344,
        "y": 6717371.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613126,
        "ID": 230613126,
        "Name": "2605",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 523268.563,
        "y": 6853359.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613127,
        "ID": 230613127,
        "Name": "2448",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 25.0
      }
    },
    {
      "geometry": {
        "x": 486409.4,
        "y": 6796897.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613128,
        "ID": 230613128,
        "Name": "1757",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 136.0
      }
    },
    {
      "geometry": {
        "x": 698794.6,
        "y": 6871316.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613129,
        "ID": 230613129,
        "Name": "3419",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 18.0
      }
    },
    {
      "geometry": {
        "x": 479629.344,
        "y": 6809817.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613130,
        "ID": 230613130,
        "Name": "2446",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 7.0
      }
    },
    {
      "geometry": {
        "x": 547830.1,
        "y": 6858767.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613131,
        "ID": 230613131,
        "Name": "2421",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 21.0
      }
    },
    {
      "geometry": {
        "x": 522193.6,
        "y": 6852597.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613132,
        "ID": 230613132,
        "Name": "2947",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 10.0
      }
    },
    {
      "geometry": {
        "x": 488184.1,
        "y": 6813487.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613133,
        "ID": 230613133,
        "Name": "7008",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 610037.5,
        "y": 6699991.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613134,
        "ID": 230613134,
        "Name": "961",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 482959.7,
        "y": 6813252.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613135,
        "ID": 230613135,
        "Name": "1778",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 459490.75,
        "y": 6795816.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613136,
        "ID": 230613136,
        "Name": "452",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 568274.563,
        "y": 6817455.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613137,
        "ID": 230613137,
        "Name": "956",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 566344.2,
        "y": 6817854.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613138,
        "ID": 230613138,
        "Name": "2454",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 487989.531,
        "y": 6813603.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613139,
        "ID": 230613139,
        "Name": "2944",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 483517.875,
        "y": 6813213.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613140,
        "ID": 230613140,
        "Name": "2648",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 587843.5,
        "y": 6837881.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613141,
        "ID": 230613141,
        "Name": "2943",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 7.0
      }
    },
    {
      "geometry": {
        "x": 482676.281,
        "y": 6813612.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613142,
        "ID": 230613142,
        "Name": "2132",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 569491.3,
        "y": 6815140.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613143,
        "ID": 230613143,
        "Name": "2413",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 584356.7,
        "y": 6725791.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613144,
        "ID": 230613144,
        "Name": "944",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 115.0
      }
    },
    {
      "geometry": {
        "x": 542065.4,
        "y": 6871038.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613145,
        "ID": 230613145,
        "Name": "2615",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 576593.6,
        "y": 6832474.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613146,
        "ID": 230613146,
        "Name": "2630",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 487988.3,
        "y": 6813596.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613147,
        "ID": 230613147,
        "Name": "2602",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488136.3,
        "y": 6813503.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613148,
        "ID": 230613148,
        "Name": "2980",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 634724.563,
        "y": 6596447.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613149,
        "ID": 230613149,
        "Name": "925",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 757247.1,
        "y": 6847341.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613150,
        "ID": 230613150,
        "Name": "2990",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 509893.781,
        "y": 6791770.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613151,
        "ID": 230613151,
        "Name": "2437",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 54.0
      }
    },
    {
      "geometry": {
        "x": 586793.9,
        "y": 6779085.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613152,
        "ID": 230613152,
        "Name": "2622",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 654924.1,
        "y": 6797787.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613153,
        "ID": 230613153,
        "Name": "455",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 664464.2,
        "y": 6602231.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613154,
        "ID": 230613154,
        "Name": "935",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 566978.6,
        "y": 6817884.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613155,
        "ID": 230613155,
        "Name": "934",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488044.3,
        "y": 6813544.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613156,
        "ID": 230613156,
        "Name": "2608",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 497766.438,
        "y": 6808809.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613157,
        "ID": 230613157,
        "Name": "2633",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 64.0
      }
    },
    {
      "geometry": {
        "x": 502771.25,
        "y": 6781610.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613158,
        "ID": 230613158,
        "Name": "2624",
        "DateGps": "2014-05-15T11:35:31.043Z",
        "Speed": 93.0
      }
    },
    {
      "geometry": {
        "x": 615483.563,
        "y": 6757685.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613160,
        "ID": 230613160,
        "Name": "1736",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 46.0
      }
    },
    {
      "geometry": {
        "x": 542253.063,
        "y": 6871015.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613167,
        "ID": 230613167,
        "Name": "924",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 651442.1,
        "y": 6771769.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613172,
        "ID": 230613172,
        "Name": "471",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 516761.563,
        "y": 6870465.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613174,
        "ID": 230613174,
        "Name": "2938",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 36.0
      }
    },
    {
      "geometry": {
        "x": 613781.25,
        "y": 6899912.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613177,
        "ID": 230613177,
        "Name": "2420",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 136.0
      }
    },
    {
      "geometry": {
        "x": 741383.063,
        "y": 6863235.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613178,
        "ID": 230613178,
        "Name": "3421",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 68.0
      }
    },
    {
      "geometry": {
        "x": 561499.3,
        "y": 6856793.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613179,
        "ID": 230613179,
        "Name": "2644",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 490843.281,
        "y": 6786028.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613180,
        "ID": 230613180,
        "Name": "2626",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 14.0
      }
    },
    {
      "geometry": {
        "x": 565571.0,
        "y": 6721113.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613181,
        "ID": 230613181,
        "Name": "945",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 482726.438,
        "y": 6813254.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613182,
        "ID": 230613182,
        "Name": "2651",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 568323.563,
        "y": 6817386.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613183,
        "ID": 230613183,
        "Name": "474",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 652207.25,
        "y": 6777085.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613184,
        "ID": 230613184,
        "Name": "2959",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 104.0
      }
    },
    {
      "geometry": {
        "x": 595569.438,
        "y": 6828853.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613185,
        "ID": 230613185,
        "Name": "933",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 615705.438,
        "y": 6757770.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613186,
        "ID": 230613186,
        "Name": "2979",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 652461.563,
        "y": 6796650.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613187,
        "ID": 230613187,
        "Name": "1760",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 97.0
      }
    },
    {
      "geometry": {
        "x": 678756.8,
        "y": 6891508.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613188,
        "ID": 230613188,
        "Name": "942",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 576564.2,
        "y": 6803176.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613189,
        "ID": 230613189,
        "Name": "2612",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 569185.6,
        "y": 6815750.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613190,
        "ID": 230613190,
        "Name": "2623",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 632457.0,
        "y": 6804846.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613191,
        "ID": 230613191,
        "Name": "2122",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 498446.781,
        "y": 6808735.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613192,
        "ID": 230613192,
        "Name": "2418",
        "DateGps": "2014-05-15T11:35:28.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 583819.25,
        "y": 6868532.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613193,
        "ID": 230613193,
        "Name": "2606",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 21.0
      }
    },
    {
      "geometry": {
        "x": 542272.0,
        "y": 6871001.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613194,
        "ID": 230613194,
        "Name": "962",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 607473.5,
        "y": 6701360.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613195,
        "ID": 230613195,
        "Name": "937",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 10.0
      }
    },
    {
      "geometry": {
        "x": 589612.7,
        "y": 6745463.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613196,
        "ID": 230613196,
        "Name": "449",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 757122.9,
        "y": 6847399.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613197,
        "ID": 230613197,
        "Name": "2955",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 568831.75,
        "y": 6816143.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613198,
        "ID": 230613198,
        "Name": "2654",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 523219.875,
        "y": 6895187.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613199,
        "ID": 230613199,
        "Name": "2120",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 589320.6,
        "y": 6744534.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613200,
        "ID": 230613200,
        "Name": "931",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 678778.0,
        "y": 6891494.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613201,
        "ID": 230613201,
        "Name": "450",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 482987.5,
        "y": 6813218.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613202,
        "ID": 230613202,
        "Name": "1770",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488219.375,
        "y": 6813412.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613203,
        "ID": 230613203,
        "Name": "2992",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 482863.5,
        "y": 6813370.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613205,
        "ID": 230613205,
        "Name": "2620",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488363.7,
        "y": 6813236.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613206,
        "ID": 230613206,
        "Name": "2993",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 482689.781,
        "y": 6813334.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613207,
        "ID": 230613207,
        "Name": "1741",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 523275.969,
        "y": 6895134.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613208,
        "ID": 230613208,
        "Name": "2117",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 678972.438,
        "y": 6891374.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613209,
        "ID": 230613209,
        "Name": "927",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 649955.25,
        "y": 6618727.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613210,
        "ID": 230613210,
        "Name": "466",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 97.0
      }
    },
    {
      "geometry": {
        "x": 650074.438,
        "y": 6618867.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613211,
        "ID": 230613211,
        "Name": "936",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 97.0
      }
    },
    {
      "geometry": {
        "x": 636287.2,
        "y": 6593133.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613212,
        "ID": 230613212,
        "Name": "950",
        "DateGps": "2014-05-15T11:35:32.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 569377.1,
        "y": 6815414.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613213,
        "ID": 230613213,
        "Name": "2459",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 567111.3,
        "y": 6720919.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613214,
        "ID": 230613214,
        "Name": "1743",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 72.0
      }
    },
    {
      "geometry": {
        "x": 480450.938,
        "y": 6812188.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613215,
        "ID": 230613215,
        "Name": "2121",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 528115.25,
        "y": 6916473.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613216,
        "ID": 230613216,
        "Name": "7007",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488057.9,
        "y": 6813675.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613217,
        "ID": 230613217,
        "Name": "2450",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 488073.2,
        "y": 6813520.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613218,
        "ID": 230613218,
        "Name": "2458",
        "DateGps": "2014-05-15T11:35:34.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 611265.3,
        "y": 6700302.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613219,
        "ID": 230613219,
        "Name": "954",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 478455.781,
        "y": 6802824.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613220,
        "ID": 230613220,
        "Name": "7063",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 566344.4,
        "y": 6817845.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613221,
        "ID": 230613221,
        "Name": "2432",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 484100.344,
        "y": 6812776.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613222,
        "ID": 230613222,
        "Name": "1737",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 651979.9,
        "y": 6626504.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613223,
        "ID": 230613223,
        "Name": "2991",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 118.0
      }
    },
    {
      "geometry": {
        "x": 496085.156,
        "y": 6717323.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613224,
        "ID": 230613224,
        "Name": "2640",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 611240.5,
        "y": 6700185.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613225,
        "ID": 230613225,
        "Name": "940",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 678682.25,
        "y": 6891558.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613226,
        "ID": 230613226,
        "Name": "464",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 566400.563,
        "y": 6817867.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613227,
        "ID": 230613227,
        "Name": "2404",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 684648.8,
        "y": 6846940.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613228,
        "ID": 230613228,
        "Name": "2942",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 481378.531,
        "y": 6814551.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613229,
        "ID": 230613229,
        "Name": "2645",
        "DateGps": "2014-05-15T11:35:30.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 651946.1,
        "y": 6769962.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613230,
        "ID": 230613230,
        "Name": "2118",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 527830.1,
        "y": 6802850.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613231,
        "ID": 230613231,
        "Name": "2988",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 568331.938,
        "y": 6818454.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613232,
        "ID": 230613232,
        "Name": "958",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 61.0
      }
    },
    {
      "geometry": {
        "x": 766877.938,
        "y": 6840392.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613234,
        "ID": 230613234,
        "Name": "3412",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 651349.9,
        "y": 6617340.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613235,
        "ID": 230613235,
        "Name": "469",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 100.0
      }
    },
    {
      "geometry": {
        "x": 590059.1,
        "y": 6873368.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613236,
        "ID": 230613236,
        "Name": "2412",
        "DateGps": "2014-05-15T11:35:35.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 611533.563,
        "y": 6700354.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613237,
        "ID": 230613237,
        "Name": "963",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 626768.6,
        "y": 6675297.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613238,
        "ID": 230613238,
        "Name": "456",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 578308.563,
        "y": 6811561.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613239,
        "ID": 230613239,
        "Name": "2649",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 32.0
      }
    },
    {
      "geometry": {
        "x": 685383.2,
        "y": 6885170.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613240,
        "ID": 230613240,
        "Name": "3413",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 100.0
      }
    },
    {
      "geometry": {
        "x": 545276.938,
        "y": 6869170.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613241,
        "ID": 230613241,
        "Name": "2949",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 667072.2,
        "y": 6654744.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613242,
        "ID": 230613242,
        "Name": "2974",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 57.0
      }
    },
    {
      "geometry": {
        "x": 676399.4,
        "y": 6890694.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613243,
        "ID": 230613243,
        "Name": "454",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 490867.25,
        "y": 6785992.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613244,
        "ID": 230613244,
        "Name": "2456",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 687928.0,
        "y": 6825738.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613245,
        "ID": 230613245,
        "Name": "2967",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 57.0
      }
    },
    {
      "geometry": {
        "x": 685390.1,
        "y": 6885163.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613246,
        "ID": 230613246,
        "Name": "3418",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 100.0
      }
    },
    {
      "geometry": {
        "x": 652109.0,
        "y": 6778848.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613247,
        "ID": 230613247,
        "Name": "1732",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 129.0
      }
    },
    {
      "geometry": {
        "x": 566619.063,
        "y": 6818728.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613248,
        "ID": 230613248,
        "Name": "2660",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 607409.938,
        "y": 6701435.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613249,
        "ID": 230613249,
        "Name": "467",
        "DateGps": "2014-05-15T11:35:37.043Z",
        "Speed": 7.0
      }
    },
    {
      "geometry": {
        "x": 611612.0,
        "y": 6700379.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613250,
        "ID": 230613250,
        "Name": "443",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 664602.2,
        "y": 6838070.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613251,
        "ID": 230613251,
        "Name": "923",
        "DateGps": "2014-05-15T11:35:36.043Z",
        "Speed": 21.0
      }
    },
    {
      "geometry": {
        "x": 733309.6,
        "y": 6865704.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613252,
        "ID": 230613252,
        "Name": "2954",
        "DateGps": "2014-05-15T11:35:37.043Z",
        "Speed": 50.0
      }
    },
    {
      "geometry": {
        "x": 490834.031,
        "y": 6786046.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613253,
        "ID": 230613253,
        "Name": "2407",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 548002.1,
        "y": 6858685.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613254,
        "ID": 230613254,
        "Name": "2639",
        "DateGps": "2014-05-15T11:35:37.043Z",
        "Speed": 28.0
      }
    },
    {
      "geometry": {
        "x": 731358.063,
        "y": 6865675.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613255,
        "ID": 230613255,
        "Name": "3408",
        "DateGps": "2014-05-15T11:35:37.043Z",
        "Speed": 79.0
      }
    },
    {
      "geometry": {
        "x": 589956.3,
        "y": 6873344.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613256,
        "ID": 230613256,
        "Name": "2625",
        "DateGps": "2014-05-15T11:35:37.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 527926.75,
        "y": 6726988.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613257,
        "ID": 230613257,
        "Name": "1742",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 79.0
      }
    },
    {
      "geometry": {
        "x": 560933.75,
        "y": 6857256.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613258,
        "ID": 230613258,
        "Name": "2950",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 39.0
      }
    },
    {
      "geometry": {
        "x": 568215.3,
        "y": 6817590.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613259,
        "ID": 230613259,
        "Name": "478",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 756470.938,
        "y": 6847556.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613260,
        "ID": 230613260,
        "Name": "3423",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 10.0
      }
    },
    {
      "geometry": {
        "x": 515736.3,
        "y": 6870617.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613261,
        "ID": 230613261,
        "Name": "2133",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 664685.063,
        "y": 6838028.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613262,
        "ID": 230613262,
        "Name": "1768",
        "DateGps": "2014-05-15T11:35:27.043Z",
        "Speed": 0.0
      }
    },
    {
      "geometry": {
        "x": 538570.938,
        "y": 6869097.0
      },
      "attributes": {
        "status": 0,
        "objectid": 230613263,
        "ID": 230613263,
        "Name": "2940",
        "DateGps": "2014-05-15T11:35:33.043Z",
        "Speed": 68.0
      }
    },
    {
      "geometry": {
        "x": 516033.0,
        "y": 6723637.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613264,
        "ID": 230613264,
        "Name": "1756",
        "DateGps": "2014-05-15T11:26:54.096Z",
        "Speed": 25.0
      }
    },
    {
      "geometry": {
        "x": 566395.063,
        "y": 6817815.5
      },
      "attributes": {
        "status": 0,
        "objectid": 230613265,
        "ID": 230613265,
        "Name": "2436",
        "DateGps": "2014-05-15T11:26:53.096Z",
        "Speed": 0.0
      }
    }
  ]
}
*/