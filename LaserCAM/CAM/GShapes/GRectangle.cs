using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GRectangle : GShape
    {
        public GRectangle(Shape shape) : base(shape) { }
        public override string ToGCode() => $"G01 ";
        public override string ToSerialize() => $"r{Canvas.GetLeft(Shape)}|{Canvas.GetBottom(Shape)}|{Shape.Width}|{Shape.Height}";
    }
}
