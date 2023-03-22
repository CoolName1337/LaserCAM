using LaserCAM.CAM.GTools;
using LaserCAM.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using Image = System.Windows.Controls.Image;

namespace LaserCAM.CAM.GShapes
{

    public abstract class GShape
    {
        public Shape Shape;
        public GShape(Shape shape)
        {
            if (shape != null)
            {
                shape.Stroke = GTool.BlackBrush;
                Shape = shape;
                GField.Panel.Children.Remove(shape);
            }
        }
        public abstract string ToGCode();
        public virtual void Create()
        {
            Shape.DataContext = this;
            GField.AddShape(this);
        }

        public abstract List<GBindingPoint> GetBindingPoints();

        public virtual void Remove()
        {
            GField.Panel.Children.Remove(Shape);
        }

        /// <summary>
        /// Affects only on color
        /// </summary>
        public virtual void Select()
        {
            Shape.Stroke = new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// Affects only on color
        /// </summary>
        public virtual void Unselect()
        {
            Shape.Stroke = new SolidColorBrush(Colors.Black);
        }

        public abstract string ToSerialize();
        public static GShape FromSerialize(string shapeData)
        {
            char shapeType = shapeData[0];
            double[] resArray = new double[0];
            if (shapeType != 'i')
                resArray = shapeData.Substring(1).Split('|').Select(str => double.Parse(str)).ToArray();
            switch (shapeType)
            {
                case 'l':
                    return new GLine(new Line() { X1 = resArray[0], Y1 = resArray[1], X2 = resArray[2], Y2 = resArray[3] });
                case 'r':
                    Rectangle rect = new() { Width = resArray[2], Height = resArray[3] };
                    Canvas.SetLeft(rect, resArray[0]);
                    Canvas.SetBottom(rect, resArray[1]);
                    return new GRectangle(rect);
                case 'e':
                    Ellipse ellipse = new() { Width = resArray[2], Height = resArray[3] };
                    Canvas.SetLeft(ellipse, resArray[0]);
                    Canvas.SetBottom(ellipse, resArray[1]);
                    return new GEllipse(ellipse);
                case 'i':
                    var imageArr = shapeData.Substring(1).Split('|');
                    var pointsArr = imageArr.Last().Split(":").Select(str=> int.Parse(str)).ToArray();
                    Bitmap bitmap = new Bitmap(int.Parse(imageArr[4]), int.Parse(imageArr[5]));

                    for (int i = 0; i < pointsArr.Length-1; i+=2)
                        bitmap.SetPixel(pointsArr[i], pointsArr[i+1], Color.Black);

                    var points = bitmap.GetPointsFromBitmap(Color.Black, true);
                    Image image = new();

                    image.Source = bitmap.ToBitmapImage();
                    image.Width = double.Parse(imageArr[2]);
                    image.Height = double.Parse(imageArr[3]);
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);

                    Canvas.SetLeft(image, double.Parse(imageArr[0]));
                    Canvas.SetBottom(image, double.Parse(imageArr[1]));

                    return new GImage(image, bitmap, points);
                case 'a':
                    var point1 = new Point(resArray[0], -resArray[1]);
                    var point2 = new Point(resArray[2], -resArray[3]);
                    Path path = new() { Stroke = new SolidColorBrush(Colors.Gray), StrokeThickness = 1 };
                    PathSegment arcSegment = new ArcSegment() { Point = point2, Size = new Size(resArray[4], resArray[4]) };
                    PathFigure arcFigure = new PathFigure(
                        point1,
                        new List<PathSegment>() { arcSegment },
                        false
                    );
                    var figures = new PathFigureCollection() { arcFigure };
                    path.Data = new PathGeometry() { Figures = figures };
                    return new GArc(path, point1, point2, resArray[4]);
            }
            return null;
        }

    }
}
