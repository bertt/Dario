using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Dario.Models
{
    public class ImageConvertor
    {
        public static byte[] Convert(Image image, string outputformat)
        {
            var memoryStream = new MemoryStream();
            var imageFormat = ImageFormat.Jpeg;
            switch (outputformat)
            {
                case "png":
                    imageFormat = ImageFormat.Png;
                    break;
                case "gif":
                    imageFormat = ImageFormat.Gif;
                    break;
            }
            image.Save(memoryStream, imageFormat);
            return memoryStream.ToArray();
        }
    }
}