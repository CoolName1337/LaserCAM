using LaserCAM.CAM.GTools;
using LaserCAM.Extensions;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GEllipse : GShape
    {
        public GEllipse(Ellipse ellipse) : base(ellipse) { }
        public GEllipse() : base(new Ellipse() { Stroke = GTool.BlackBrush, StrokeThickness = 1 }) { }
        public override string ToGCode() {
            var pos = new Point(Canvas.GetLeft(Shape) - GPoint.Position.X, Canvas.GetBottom(Shape) - GPoint.Position.Y);
            return 
                $"G00 X{pos.X.Round()} " +
                $"Y{(pos.Y + Shape.Height / 2).Round()}\n" +
                $"G02 X{pos.X.Round()} " +
                $"Y{(pos.Y + Shape.Height / 2).Round()} I{Shape.Width / 2} J0";
            }
        public override string ToSerialize() => $"e{Canvas.GetLeft(Shape)}|{Canvas.GetBottom(Shape)}|{Shape.Width}|{Shape.Height}";
    }
}
