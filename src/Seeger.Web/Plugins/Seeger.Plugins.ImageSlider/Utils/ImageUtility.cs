using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using Seeger;

namespace Seeger.Plugins.ImageSlider.Utils
{
    public class ImageUtility
    {
        public static void Crop(string originalImagePath, string savePath, Size cropWindowSize, int offsetLeft, int offsetTop, Size imageSize)
        {
            Image sourceImage = null;
            Image cropImage = null;

            try
            {
                var originalImg = Image.FromFile(originalImagePath);

                if (originalImg.Width != imageSize.Width || originalImg.Height != imageSize.Height)
                {
                    sourceImage = CreateThumbnail(originalImg, imageSize.Width, imageSize.Height, ThumbLagerThanOriginalImageBehavior.ZoomOut);
                }
                else
                {
                    sourceImage = originalImg;
                }

                cropImage = new Bitmap(cropWindowSize.Width, cropWindowSize.Height);

                using (var g = Graphics.FromImage(cropImage))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.Clear(Color.Transparent);

                    g.DrawImage(sourceImage,
                        new Rectangle(0, 0, cropWindowSize.Width, cropWindowSize.Height),
                        new Rectangle(new Point(offsetLeft, offsetTop), cropWindowSize),
                        GraphicsUnit.Pixel);

                    cropImage.Save(savePath, ImageFormat.Jpeg);
                }

                originalImg.Dispose();
            }
            finally
            {
                if (sourceImage != null)
                {
                    sourceImage.Dispose();
                }
                if (cropImage != null)
                {
                    cropImage.Dispose();
                }
            }
        }

        public static void Zoom(string sourceImagePath, int maxWidth)
        {
            var tempPath = Path.Combine(Path.GetDirectoryName(sourceImagePath), "temp_" + Path.GetFileName(sourceImagePath));

            try
            {
                File.Copy(sourceImagePath, tempPath);
                CreateThumbnail(tempPath, sourceImagePath, maxWidth, ThumbLagerThanOriginalImageBehavior.ZoomOut);
            }
            finally
            {
                IOUtil.EnsureFileDeleted(tempPath);
            }
        }

        public static Image CreateThumbnail(Image sourceImage, int width, int height, ThumbLagerThanOriginalImageBehavior behavior)
        {
            if (sourceImage.Width <= width && sourceImage.Height <= height)
            {
                if (behavior == ThumbLagerThanOriginalImageBehavior.Ignore)
                {
                    return sourceImage;
                }
            }

            var thumb = new Bitmap(width, height);

            var oldWidth = sourceImage.Width;
            var oldHeight = sourceImage.Height;

            using (var g = Graphics.FromImage(thumb))
            {
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);

                g.DrawImage(sourceImage, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, oldWidth, oldHeight),
                    GraphicsUnit.Pixel);
            }

            return thumb;
        }

        public static void CreateThumbnail(string sourceImagePath, string thumbSavePath, int thumbWidth, ThumbLagerThanOriginalImageBehavior behavior)
        {
            using (var img = Image.FromFile(sourceImagePath))
            {
                if (img.Width <= thumbWidth)
                {
                    img.Save(thumbSavePath);
                }
                else
                {
                    var thumbHeight = img.Height * thumbWidth / img.Width;

                    using (var thumb = CreateThumbnail(img, thumbWidth, thumbHeight, behavior))
                    {
                        thumb.Save(thumbSavePath);
                    }
                }
            }
        }

        public static void CreateThumbnail(string sourceImagePath, string thumbSavePath, int thumbMaxWidth, int thumbMaxHeight, ThumbLagerThanOriginalImageBehavior behavior)
        {
            using (var img = Image.FromFile(sourceImagePath))
            {
                if (img.Width <= thumbMaxWidth && img.Height <= thumbMaxHeight)
                {
                    img.Save(thumbSavePath);
                }
                else
                {
                    var oldImageFactor = (double)img.Width / (double)img.Height;
                    var newImageFactor = (double)thumbMaxWidth / (double)thumbMaxHeight;

                    var newWidth = 0;
                    var newHeight = 0;

                    if (oldImageFactor > newImageFactor)
                    {
                        newWidth = thumbMaxWidth;
                        newHeight = img.Height * thumbMaxWidth / img.Width;
                    }
                    else
                    {
                        newHeight = thumbMaxHeight;
                        newWidth = img.Width * thumbMaxHeight / img.Height;
                    }

                    using (var thumb = CreateThumbnail(img, newWidth, newHeight, behavior))
                    {
                        thumb.Save(thumbSavePath);
                    }
                }
            }
        }

        public static void AddWaterMark(string imagePath, string text)
        {
            var tempPath = Path.Combine(Path.GetDirectoryName(imagePath), "temp_" + Path.GetFileName(imagePath));
            File.Copy(imagePath, tempPath);

            using (var image = Image.FromFile(tempPath))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    g.DrawImage(image, 0, 0, image.Width, image.Height);

                    var waterMarkRec = new Rectangle(0, image.Height - 18, image.Width, 18);

                    var bgBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                    g.FillRectangle(bgBrush, waterMarkRec);

                    var font = new Font("Verdana", 10, GraphicsUnit.Pixel);
                    var fontBrush = new SolidBrush(Color.Black);

                    g.DrawString(text, font, fontBrush, 5, image.Height - 15);
                }

                image.Save(imagePath, ImageFormat.Jpeg);
            }

            IOUtil.EnsureFileDeleted(tempPath);
        }
    }
}
