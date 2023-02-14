using LaserCAM.CAM.GTools;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{

    public abstract class GShape
    {
        public Shape Shape;
        public GShape(Shape shape)
        {
            if(shape != null)
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
            double[] resArray = shapeData.Substring(1).Split('|').Select(str => double.Parse(str)).ToArray();
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
            }
            return null;
        }

    }
}
