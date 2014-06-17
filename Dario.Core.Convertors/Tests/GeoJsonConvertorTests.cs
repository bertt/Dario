using Dario.Core.GeoJson;
using NUnit.Framework;

namespace Dario.Core.Convertors.Tests
{
    public class GeoJsonConvertorTests
    {
        [Test]
        public void TestConvertFeature()
        {
            var feature = new Feature();

            // arrange
            var geom = new Geometry {type = "Polygon"};
            double[,] numbers = {{1, 2}, {3, 4}};
            geom.coordinates = numbers;
            feature.geometry = geom;
            feature.id = "1";
            feature.Properties.Add("LOWPARCELID", "1902226080");

            // act
            var result = feature.ToEsriJson();

            // assert
            Assert.IsTrue(result.attributes.Count == 2);
            Assert.IsTrue(result.geometry.rings!=null);
        }

        [Test]
        public void TestConvertFeatureCollection()
        {
            // arrange
            var featureCollection = new FeatureCollection();
            var geom = new Geometry { type = "Polygon" };
            double[,] numbers = { { 1, 2 }, { 3, 4 } };
            geom.coordinates = numbers;
            var feature = new Feature {geometry = geom};
            feature.id = "1";
            feature.Properties.Add("LOWPARCELID", "1902226080");
            featureCollection.features.Add(feature);

            // act
            var result = featureCollection.ToEsriJJson();

            // assert
            Assert.IsTrue(result.geometryType == "esriGeometryPolygon");
        }


    }
}
