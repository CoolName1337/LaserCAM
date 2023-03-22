using LaserCAM.CAM.GTools;
using LaserCAM.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GLine : GShape
    {
        public GLine(Line line) : base(line) { }
        public GLine() : base(new Line() { Stroke = GTool.BlackBrush, StrokeThickness = 1 }) { }
        public override string ToGCode()
        {
            if (Shape is Line line)
                return $"G00 X{(line.X1 - GZeroPoint.Position.X).Round()} Y{(-line.Y1 - GZeroPoint.Position.Y).Round()}\n" +
                        $"G01 X{(line.X2 - GZeroPoint.Position.X).Round()} Y{(-line.Y2 - GZeroPoint.Position.Y).Round()}";
            return "";
        }
        public override string ToSerialize()
        {
            if (Shape is Line line)
                return $"l{line.X1}|{line.Y1}|{line.X2}|{line.Y2}";
            return "";
        }

        public override List<GBindingPoint> GetBindingPoints()
        {
            if(Shape is Line line)
            {
                return new List<GBindingPoint>()
                {
                    new GBindingPoint((line.X1 + line.X2)/2, (-line.Y1 - line.Y2) / 2, GBindingPointType.Center),
                    new GBindingPoint(line.X1, -line.Y1, GBindingPointType.Vertex),
                    new GBindingPoint(line.X2, -line.Y2, GBindingPointType.Vertex),
                };
            }
            return null;

        }
    }
}
