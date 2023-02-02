using LaserCAM.CAM.GTools;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GEllipse : GShape
    {
        public GEllipse(Ellipse ellipse) : base(ellipse) { }
        public GEllipse() : base(new Ellipse() { Stroke = GTool.BlackBrush, StrokeThickness = 1 }) { }
        public override string ToGCode()
        {
            return $"G02 ";
        }
    }
}
