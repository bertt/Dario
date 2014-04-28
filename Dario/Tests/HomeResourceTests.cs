using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using NUnit.Framework;

namespace Dario.Tests
{
    public class HomeResourceTests
    {
        private const string Testserver = "http://localhost:1223";

        [Test]
        public async void HomeTest()
        {
            using (WebApp.Start<Startup>(Testserver))
            {
                var httpclient = new HttpClient{ BaseAddress = new Uri(Testserver) };
                var response = await httpclient.GetAsync("api");
                var res = await response.Content.ReadAsStringAsync();
                Assert.True(res.Contains("home"));
            }
        }
    }
}