using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dario.Models
{
    public class ImageMerger
    {
        public static Image Merge(List<Image> images)
        {
            if (images == null) throw new ArgumentNullException();
            var compositeImage = new Bitmap(images[0].Width, images[0].Height, PixelFormat.Format32bppArgb);
            var compositeGraphics = Graphics.FromImage(compositeImage);
            foreach (var image in images)
            {
                compositeGraphics.DrawImage(image, new Point(0, 0));
            }
            return compositeImage;
        }

    }
}