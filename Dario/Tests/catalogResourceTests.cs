using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Dario.Tests
{
    public class CatalogResourceTests
    {
        private const string Testserver = "http://localhost:12233";

        [Test]
        public async void TileTest()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync("/rest/services");
                var stream = await response.Content.ReadAsStreamAsync();
                Assert.IsNotNull(stream);
            }
        }

    }
}