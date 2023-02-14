using LaserCAM.CAM.GShapes;
using LaserCAM.Extensions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace LaserCAM.CAM.GTools
{
    public class GToolImage : GTool
    {
        List<Point> points = new();
        double aspect;
        BitmapImage bitmapImage;
        Bitmap bitmap;
        Image image { get; set; } = new Image();
        public GToolImage(string fileName)
        {
            bitmap = new Bitmap(fileName);

            points = bitmap.GetPointsFromBitmap(Color.Black, true);
            bitmapImage = bitmap.ToBitmapImage();

            aspect = bitmap.Height / (double)bitmap.Width;
            image.Height = bitmap.Height;
            image.Width = bitmap.Width;
            image.Source = bitmapImage;

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            GField.Panel.Children.Add(image);

            SetNormalSize();
        }
        private void SetNormalSize()
        {

            foreach (FrameworkElement el in (ParamsWindow.Child as Grid).Children)
            {
                if (el.Tag == "d" && el is TextBox tb)
                    tb.Text = bitmap.Width.ToString();
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetBottom(image, Cursor.Y - image.Height / 2);
            Canvas.SetLeft(image, Cursor.X - image.Width / 2);
        }

        public override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                OnLeftMouseDown();
            }
        }
        public override void OnLeftMouseDown()
        {
            GImage gimage = new(image, bitmap, points);
            gimage.Create();
            InitializeShape();
        }


        public override void InitializeShape()
        {
            if(bitmapImage != null)
            {
                image = new Image();
                image.Source = bitmapImage.Clone();
                image.Height = bitmapImage.Height;
                image.Width = bitmapImage.Width;
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
                GField.Panel.Children.Add(image);
                SetParams();
            }
        }

        public override void RemoveShape() {
            GField.Panel.Children.Remove(image);
        }

        public override void SetParams()
        {
            Canvas.SetBottom(image, Cursor.Y - image.Height / 2);
            Canvas.SetLeft(image, Cursor.X - image.Width / 2);
            image.Width = ParamValues["d"];
            image.Height = ParamValues["d"] / aspect;
        }
    }
}
