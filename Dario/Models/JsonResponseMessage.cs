using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Dario.Models
{
    public static class JsonResponseMessage
    {
        public static HttpResponseMessage GetHttpResponseMessage(string content, string contentType, HttpStatusCode code)
        {
            var httpResponseMessage = new HttpResponseMessage { Content = new StringContent(content) };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            return httpResponseMessage;

        }
        public static HttpResponseMessage GetHttpResponseMessage(byte[] content, string contentType, HttpStatusCode code)
        {
            var httpResponseMessage = new HttpResponseMessage { Content = new ByteArrayContent(content) };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            return httpResponseMessage;

        }

    }
}