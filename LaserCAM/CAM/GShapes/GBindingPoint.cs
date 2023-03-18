using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LaserCAM.CAM.GShapes
{
    public enum GBindingPointType
    {
        Center,
        Vertex,
        Edge,
    }
    public struct GBindingPoint
    {
        public Point Point { get; set; } = new Point();
        public GBindingPointType Type { get; set; } = GBindingPointType.Vertex;
        public GBindingPoint() { }
        public GBindingPoint(double x, double y, GBindingPointType type) : this(new Point(x,y), type) { }
        public GBindingPoint(Point point, GBindingPointType type)
        {
            Point = point;
            Type = type;
        }

    }
}
