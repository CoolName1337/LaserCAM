using LaserCAM.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace LaserCAM.CAM.GShapes
{

    public class GImage : GShape
    {
        public Image Image { get; init; }
        public List<Point> Points;
        public Bitmap Bitmap { get; init; }
        public GImage(Image image, Bitmap bitmap, List<Point> points) : base(null) {
            Image = image;
            Points = points;
            Bitmap = bitmap;
            GField.Panel.Children.Remove(Image);
        }

        public override void Select()
        {
            Bitmap.SetColor(Color.Black, Color.Red);
            Image.Source = Bitmap.ToBitmapImage();
        }

        public override void Unselect()
        {

            Bitmap.SetColor(Color.Red, Color.Black);
            Image.Source = Bitmap.ToBitmapImage();
        }

        public List<Point> GetPoints()
        {
            var res = Points.Select(p =>
            {
                p.X += Canvas.GetLeft(Image);
                p.Y += Canvas.GetBottom(Image);
                return p;
            });
            return res.ToList();
        }

        public override void Remove()
        {
            GField.Panel.Children.Remove(Image);
        }

        public override void Create()
        {
            Image.DataContext = this;
            GField.AddShape(this);
        }

        public override string ToGCode()
        {
            throw new NotImplementedException();
        }

        public override string ToSerialize()
        {
            throw new NotImplementedException();
        }
    }
}
