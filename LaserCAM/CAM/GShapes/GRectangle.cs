using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GRectangle : GShape
    {
        public GRectangle(Shape shape) : base(shape) { }
        public override string ToGCode()
        {
            return "G01 ";
        }
    }
}
