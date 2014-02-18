using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Dario.Tests
{
    public class VersionResourceTests
    {
        private const string Testserver = "http://localhost:1223";

        [Test]
        public async void VersionTest()
        {
            var i = WebApp.Start<Startup>(Testserver);
            using (i)
            {
                var httpclient = new HttpClient { BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync("api/version");
                var res = await response.Content.ReadAsStringAsync();
                Assert.True(res.Contains("0.1"));
            }
        }

    }
}
