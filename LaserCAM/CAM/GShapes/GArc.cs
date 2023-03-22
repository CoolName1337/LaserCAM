using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LaserCAM.Extensions;

namespace LaserCAM.CAM.GShapes
{
    internal class GArc : GShape
    {
        private Point _point1;
        private Point _point2;
        private double _radius;
        public GArc(Shape shape, Point point1, Point point2, double radius) : base(shape)
        {
            double delta = Math.Round(
                    Math.Abs((point1 - point2).Length/2),
                        2,
                        MidpointRounding.ToPositiveInfinity
                    );
            radius = radius < delta ? delta : radius;

            _point1 = new Point(point1.X, -point1.Y);
            _point2 = new Point(point2.X, -point2.Y);

            _radius = radius;
        }

        public override List<GBindingPoint> GetBindingPoints()
        {
            return new List<GBindingPoint>()
            {
                new GBindingPoint(_point1, GBindingPointType.Vertex),
                new GBindingPoint(_point2, GBindingPointType.Vertex),
            };
        }

        public override string ToGCode()
        {
            var point1 = new Point(_point1.X - GZeroPoint.Position.X, _point1.Y - GZeroPoint.Position.Y).Round();
            var point2 = new Point(_point2.X - GZeroPoint.Position.X, _point2.Y - GZeroPoint.Position.Y).Round();
            return
                $"G00 X{point2.X} " +
                $"Y{point2.Y}\n" +
                $"G02 X{point1.X} " +
                $"Y{point1.Y} R{_radius}";
        }

        public override string ToSerialize() => $"a{_point1.X}|{_point1.Y}|{_point2.X}|{_point2.Y}|{_radius}";
    }
}
