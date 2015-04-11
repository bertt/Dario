using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Dario.Models
{
    public class ImageMerger
    {
        public static byte[] Merge(List<byte[]> images, string ext)
        {
            Graphics compositeGraphics = null;
            Image compositeImage = null;

            if (images == null) throw new ArgumentNullException();

            try
            {
                foreach (var image in images)
                {
                    if (compositeImage == null) // first time
                    {
                        compositeImage = Image.FromStream(new MemoryStream(image));
                        compositeGraphics = Graphics.FromImage(compositeImage);
                    }
                    else
                    {
                        using (var singleImage = Image.FromStream(new MemoryStream(image)))
                        {
                            compositeGraphics.DrawImage(singleImage, new Point(0, 0));
                        }
                    }
                }
                return ImageConvertor.Convert(compositeImage, ext);
            }
            finally
            {
                if (compositeGraphics != null) compositeGraphics.Dispose();
                if (compositeImage != null) compositeImage.Dispose();
            }
        }
    }
}