using LaserCAM.CAM.GTools;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GLine : GShape
    {
        public GLine(Line line) : base(line) { }
        public GLine() : base(new Line() { Stroke = GTool.BlackBrush, StrokeThickness = 1 }) { }
        public override string ToGCode() =>  $"G01 ";
        public override string ToSerialize()
        {
            if(Shape is Line line)
                return $"l{line.X1}|{line.Y1}|{line.X2}|{line.Y2}";
            return "";
        }
    }
}
