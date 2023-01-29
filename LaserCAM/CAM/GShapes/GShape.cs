using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public abstract class GShape
    {
        public Shape Shape;
        public abstract string ToGCode();
        public virtual void Create()
        {
            Shape.DataContext = this;
            GField.Panel.Children.Add(Shape);
        }

    }
}
