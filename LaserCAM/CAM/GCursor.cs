using LaserCAM.CAM.GTools;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private static Line verticalLine = new() { Stroke = GTool.GrayBrush, StrokeThickness = 1 };
        private static Line horizontalLine = new() { Stroke = GTool.GrayBrush, StrokeThickness = 1 };

        private static bool isAim = false;
        public static bool IsAim
        {
            get => isAim;
            set
            {
                isAim = value;
                if (isAim)
                {
                    GField.Panel.Children.Add(verticalLine);
                    GField.Panel.Children.Add(horizontalLine);
                }
                else{
                    GField.Panel.Children.Remove(verticalLine);
                    GField.Panel.Children.Remove(horizontalLine);
                }
            }
        }

        private static GTool _selectedTool;
        public static GTool SelectedTool
        {
            get => _selectedTool; 
            set
            {
                _selectedTool?.RemoveShape();
                _selectedTool = value;
            }
        }
        private static Point _position;
        public static Point Position
        {
            get => _position;
            set
            {
                // Set calculated position

                var point = (new Point(-GField.Position.X, GField.Position.Y) - new Vector(-value.X, value.Y)).Divide(GField.KSize);
                _position = new Point(Math.Round(point.X,2), Math.Round(point.Y,2));

                // Set position for aim

                verticalLine.X1 = verticalLine.X2 = _position.X;

                verticalLine.Y1 = GField.MainPanel.ActualHeight / GField.KSize - _position.Y;
                verticalLine.Y2 = -GField.MainPanel.ActualHeight / GField.KSize - _position.Y;

                horizontalLine.Y1 = horizontalLine.Y2 = -_position.Y;

                horizontalLine.X1 = -GField.MainPanel.ActualWidth / GField.KSize + _position.X;
                horizontalLine.X2 = GField.MainPanel.ActualWidth / GField.KSize + _position.X;

            }
        }


    }
}
