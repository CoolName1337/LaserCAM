using LaserCAM.CAM.GShapes;
using LaserCAM.CAM.GTools;
using System.Windows;

namespace LaserCAM.CAM
{

    public static class PointExsteder
    {
        public static Point Multiply(this Point p, double d)
        {
            p.X *= d;
            p.Y *= d;
            return p;
        }
        public static Point Divide(this Point p, double d)
        {
            p.X /= d;
            p.Y /= d;
            return p;
        }
    }

    public static class GCursor
    {
        public static GTool SelectedTool { get; set; }
        private static Point _position;
        public static Point Position
        {
            get => _position; 
            set
            {
                _position = (value - (Vector)GField.Position).Divide(GField.KSize);
            }
        }

        

    }
}
