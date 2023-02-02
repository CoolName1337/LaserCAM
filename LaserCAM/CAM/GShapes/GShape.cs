using LaserCAM.CAM.GTools;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public abstract class GShape
    {
        public Shape Shape;
        public GShape(Shape shape)
        {
            shape.Stroke = GTool.BlackBrush;
            Shape = shape;
            GField.Panel.Children.Remove(shape);
        }
        public abstract string ToGCode();
        public virtual void Create()
        {
            Shape.DataContext = this;
            GField.Panel.Children.Add(Shape);
        }

    }
}
