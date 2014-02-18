using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Dario.Tests
{
    public class TileResourceTests
    {
        private const string Testserver = "http://localhost:1223";

        [Test]
        public async void TileTest()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));
                var response = await httpclient.GetAsync("api/tile");
                var stream = await response.Content.ReadAsStreamAsync();
                var image = Image.FromStream(stream);
                Assert.True(image.Width>0);
            }
        }

        [Test]
        public async void TileWithExtTest()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync("/api/tile.png");
                var stream = await response.Content.ReadAsStreamAsync();
                var image = Image.FromStream(stream);
                Assert.True(image.Width > 0);
            }
        }
    }
}
