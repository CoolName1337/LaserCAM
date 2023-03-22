using LaserCAM.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace LaserCAM.CAM.GShapes
{
    enum GPointState
    {
        IsFirst,
        IsLast,
        IsDot
    }

    public class GImage : GShape
    {
        private double kSize;

        private Bitmap selectedBitmap;
        public Image Image { get; init; }
        public List<Point> Points;
        public Bitmap Bitmap { get; init; }
        public GImage(Image image, Bitmap bitmap, List<Point> points) : base(null) {
            Image = image;
            Points = points;
            Bitmap = bitmap.Clone() as Bitmap;
            GField.Panel.Children.Remove(Image);
            kSize = Image.Width / Bitmap.Width;

            selectedBitmap = Bitmap.Clone() as Bitmap;
            Bitmap.SetColor(Color.Black, Color.Red);
        }

        public override void Select()
        {
            Image.Source = Bitmap.ToBitmapImage();
        }

        public override void Unselect()
        {
            Image.Source = selectedBitmap.ToBitmapImage();
        }

        /// <summary>
        /// Returns with very cool genius algorithm... so... returns points for G-code and list of bool that means is point was last(true) or first(false)
        /// </summary>
        private (List<Point>, List<GPointState>) GetOptimizedAndSizedPoints()
        {
            bool isFirst = false;
            bool isLast = false;
            var points = GetPositionedPoints();
            int len = points.Count;
            var list = new List<Point>();
            var listLast = new List<GPointState>();
            for (int i = 0; i < len; i++)
            {
                isLast = i == len-1 ? true : points[i].X != points[i + 1].X || points[i].Y + 1 != points[i + 1].Y;
                isFirst = i == 0 ? true : points[i - 1].X != points[i].X || points[i - 1].Y + 1 != points[i].Y;

                if (isFirst || isLast)
                {
                    listLast.Add(isLast && isFirst ? GPointState.IsDot : isFirst ? GPointState.IsFirst : GPointState.IsLast);
                    list.Add(points[i].Multiply(kSize));
                }
            }
            return (list, listLast);
        }

        public List<Point> GetPositionedPoints()
        {
            var res = Points.Select(p =>
            {
                p.X += Canvas.GetLeft(Image)/kSize - GZeroPoint.Position.X;
                p.Y -= Canvas.GetBottom(Image) / kSize - GZeroPoint.Position.Y;
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
            StringBuilder strBuilder = new();

            (var points, var pointStates) = GetOptimizedAndSizedPoints();
            for (int i = 0; i < points.Count; i++)
            {
                var p = new Point(points[i].X, -points[i].Y + Bitmap.Height * kSize).Round(4);
                if(pointStates[i] == GPointState.IsDot)
                    strBuilder.Append($"G00 X{p.X} Y{p.Y}\nG01 X{p.X} Y{p.Y}\n");
                else
                    strBuilder.Append($"{(pointStates[i] == GPointState.IsFirst ? "G00":"G01")} X{p.X} Y{p.Y}\n");
            }
            return strBuilder.ToString();
        }

        public override string ToSerialize()
        {
            return $"i{Canvas.GetLeft(Image)}|{Canvas.GetBottom(Image)}|{Image.Width}|{Image.Height}|" +
                $"{Bitmap.Width}|{Bitmap.Height}|{string.Join(":", Points).Replace(";",":")}";
        }

        public override List<GBindingPoint> GetBindingPoints()
        {
            return null;
        }
    }
}
