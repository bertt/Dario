using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dario.Tests
{
    public class FeatureServerResourceTests
    {
        private const string Testserver = "http://localhost:12233";

        [Test]
        public async void GetFeatureServerReturnsFeatureServer()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync("/rest/services/Treinen/FeatureServer");
                var res = await response.Content.ReadAsStringAsync();
                var d= JsonConvert.DeserializeObject(res);
                Assert.IsNotNull(d);
            }
        }

        [Test]
        public async void GetFeaturesReturnsFeatures()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync("rest/services/countries/FeatureServer/0/query/query?returngeometry=true&spatialRel=esriSpatialRelIntersects&where=1%3d1&outSR=102100&maxAllowableOffset=38.2185141425367&outFields=*&orderByFields=ID+ASC&f=json");
                var res = await response.Content.ReadAsStringAsync();
                var d = JsonConvert.DeserializeObject(res);
                Assert.IsNotNull(d);
            }
        }

        [Test]
        public async void GetFeaturesWithgeometryFilterReturnsLessFeatures()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync(@"rest/services/Countries/FeatureServer/0/query?f=json&returnGeometry=true&spatialRel=esriSpatialRelIntersects&maxAllowableOffset=19567&geometry={'xmin':-0.000004988163709640503,'ymin':1947801.0985411945,'xmax':10018754.171386972,'ymax':11966555.269933153,'spatialReference':{'wkid':102100,'latestWkid':3857}}&geometryType=esriGeometryEnvelope&inSR=102100&outFields=&outSR=102100");
                var res = await response.Content.ReadAsStringAsync();
                var d = JsonConvert.DeserializeObject(res);
                Assert.IsNotNull(d);
            }
        }

        // todo: [Test]
        public async void GetFeaturesFromPostgisReturnsFeatures()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync(@"rest/services/treinen/FeatureServer/0/query/query?returngeometry=true&spatialRel=esriSpatialRelIntersects&where=1%3d1&outSR=102100&maxAllowableOffset=38.2185141425367&outFields=*&orderByFields=ID+ASC&f=json");
                var res = await response.Content.ReadAsStringAsync();
                var d = JsonConvert.DeserializeObject(res);
                Assert.IsNotNull(d);
            }
        }



    }
}