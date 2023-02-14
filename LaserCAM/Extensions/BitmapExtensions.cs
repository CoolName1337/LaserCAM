using System.Collections.Generic;
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace LaserCAM.Extensions
{
    public static class BitmapExtensions
    {

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            MemoryStream ms = new();
            bitmap.Save(ms, ImageFormat.Png);

            BitmapImage bitmapImage = new();

            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        /// <summary>
        /// Returns positions of pixels with color and if toChange is true set other pixels of bitmap to transperent
        /// </summary>
        public static List<Point> GetPointsFromBitmap(this Bitmap bitmap, Color color, bool toChange = false)
        {
            List<Point> points = new();
            bitmap.MakeTransparent();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    if (pixel.ToArgb() == color.ToArgb())
                    {
                        points.Add(new Point(i, j));
                    }
                    else if (toChange)
                    {
                        bitmap.SetPixel(i, j, Color.Transparent);
                    }
                }
            }
            return points;
        }

        public static void SetColor(this Bitmap bitmap, Color pastColor, Color newColor)
        {
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var px = bitmap.GetPixel(i, j);
                    if (px.ToArgb() == pastColor.ToArgb())
                        bitmap.SetPixel(i, j, newColor);
                }
            }
        }
    }
}
