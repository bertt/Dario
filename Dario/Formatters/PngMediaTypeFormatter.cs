using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Dario.Formatters
{
    public class PngMediaTypeFormatter : MediaTypeFormatter
    {
        public PngMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            MediaTypeMappings.Add(new UriPathExtensionMapping("png", "image/png"));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var taskSource = new TaskCompletionSource<object>();
            var image = (Image)value;
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            var bytes = ms.ToArray();
            writeStream.Write(bytes, 0, bytes.Length);
            taskSource.SetResult(null);
            return taskSource.Task;
        }


        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return (type == typeof(Image));
        }
    }
}