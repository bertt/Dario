using Newtonsoft.Json;
using NUnit.Framework;

namespace Dario.Core.GeoJson.Tests
{
    public class GeoJsonTests
    {
        [Test]
        public void TestReadSampleGeoJsonFileWithPolygon()
        {
            // arrange
            var jsonstring = @"{'type':'FeatureCollection','features':[{'type':'Feature','geometry':
            {'type':'Polygon','coordinates':[[[-83.23145862723665,42.61719935058299],[-83.2316352368618,42.61737853618379],[-83.23166951279292,42.61742295755952],[-83.23119346860754,42.617678900083554],[-83.23097637034452,42.6174586310034],[-83.23145862723665,42.61719935058299]]]},'properties':{'LOWPARCELID':'1902226080'}}]}";

            // act
            var d = JsonConvert.DeserializeObject<GeoJson>(jsonstring);

            // assert
            Assert.IsTrue(d != null);
            // var coord= d.features[0].
        }

        [Test]
        public void TestReadCowGeoJsonFileWithPoint()
        {
            // arrange
            var jsonstring = @"{
  'type': 'Feature',
  'properties': {
    'icon': './images/mapicons/mapicons/comment-map-icon.png',
    'key': '1400153553960_1400153942505',
    'creator': 'Tom',
    'owner': 'Tom'
  },
  'geometry': {
    'type': 'Point',
    'coordinates': [
      5.10040283203125,
      52.087734049322428
    ]
  },
  'id': '1400153553960_1400153942505',
  'style': {
    'icon': './images/mapicons/mapicons/comment-map-icon.png',
    'fill-opacity': 0.5,
    'opacity': 1
  }
}";

            // act
            var d = JsonConvert.DeserializeObject<Feature>(jsonstring);

            // assert
            Assert.IsTrue(d != null);
        }

    }
}
