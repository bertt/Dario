using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dario.Tests
{
    public class FeatureServerResourceTests
    {
        // rest/services/Treinen/FeatureServer
        private const string Testserver = "http://localhost:12233";

        [Test]
        public async void TileTest()
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


    }
}