using LaserCAM.CAM.GTools;
using LaserCAM.Extensions;
using System;
using System.Windows;
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
                return $"G00 X{(line.X1 - GPoint.Position.X).Round()} Y{(-line.Y1 - GPoint.Position.Y).Round()}\n" +
                        $"G01 X{(line.X2 - GPoint.Position.X).Round()} Y{(-line.Y2 - GPoint.Position.Y).Round()}";
            return "";
        }
        public override string ToSerialize()
        {
            if (Shape is Line line)
                return $"l{line.X1}|{line.Y1}|{line.X2}|{line.Y2}";
            return "";
        }
    }
}
