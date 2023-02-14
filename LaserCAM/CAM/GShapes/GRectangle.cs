using LaserCAM.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GRectangle : GShape
    {
        public GRectangle(Shape shape) : base(shape) { }
        public override string ToGCode()
        {
            var pos = new Point(
                Canvas.GetLeft(Shape) - GPoint.Position.X, 
                Canvas.GetBottom(Shape) - GPoint.Position.Y
                ).Round();
            return
            $"G00 X{pos.X} Y{pos.Y}\n" +
            $"G01 X{(pos.X + Shape.Width).Round()} Y{pos.Y}\n" +
            $"G01 X{(pos.X + Shape.Width).Round()} Y{(pos.Y + Shape.Height).Round()}\n" +
            $"G01 X{pos.X} Y{(pos.Y + Shape.Height).Round()}\n" +
            $"G01 X{pos.X} Y{pos.Y}";
        }
        public override string ToSerialize() => $"r{Canvas.GetLeft(Shape)}|{Canvas.GetBottom(Shape)}|{Shape.Width}|{Shape.Height}";
    }
}
