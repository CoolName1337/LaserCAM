using LaserCAM.CAM.GTools;
using System.Drawing;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GLine : GShape
    {
        public GLine(Line line) : base(line) { }
        public GLine() : base(new Line() { Stroke = GTool.BlackBrush, StrokeThickness = 1 }) { }
        public override string ToGCode()
        {
            return $"G01 ";
        }
    }
}
