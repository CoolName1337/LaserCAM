using LaserCAM.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GShapes
{
    public class GRectangle : GShape
    {
        public GRectangle(Shape shape) : base(shape) { }

        public override List<GBindingPoint> GetBindingPoints()
        {
            var pos = new Point(Canvas.GetLeft(Shape), Canvas.GetBottom(Shape));

            return new List<GBindingPoint>()
            {
                new GBindingPoint(pos.X + Shape.Width / 2, pos.Y + Shape.Height / 2, GBindingPointType.Center),

                new GBindingPoint(pos.X, pos.Y + Shape.Height / 2, GBindingPointType.Edge),
                new GBindingPoint(pos.X + Shape.Width / 2, pos.Y, GBindingPointType.Edge),
                new GBindingPoint(pos.X + Shape.Width/2, pos.Y + Shape.Height, GBindingPointType.Edge),
                new GBindingPoint(pos.X + Shape.Width, pos.Y+ Shape.Height/2, GBindingPointType.Edge),

                new GBindingPoint(pos.X, pos.Y, GBindingPointType.Vertex),
                new GBindingPoint(pos.X + Shape.Width, pos.Y + Shape.Height, GBindingPointType.Vertex),
                new GBindingPoint(pos.X, pos.Y + Shape.Height, GBindingPointType.Vertex),
                new GBindingPoint(pos.X + Shape.Width, pos.Y, GBindingPointType.Vertex),
            };
        }

        public override string ToGCode()
        {
            var pos = new Point(
                Canvas.GetLeft(Shape) - GZeroPoint.Position.X, 
                Canvas.GetBottom(Shape) - GZeroPoint.Position.Y
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
