using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM.CAM
{
    public static class GPoint
    {
        private static Point _position;
        public static Point Position
        {
            get => _position;
            set
            {
                _position = value;

                verticalLine.X1 = verticalLine.X2 = _position.X;

                verticalLine.Y1 = -GField.MainPanel.ActualHeight / GField.KSize + _position.Y;
                verticalLine.Y2 = GField.MainPanel.ActualHeight / GField.KSize + _position.Y;

                horizontalLine.Y1 = horizontalLine.Y2 = _position.Y;

                horizontalLine.X1 = -GField.MainPanel.ActualWidth / GField.KSize + _position.X;
                horizontalLine.X2 = GField.MainPanel.ActualWidth / GField.KSize + _position.X;
            }
        }

        private static bool isActive = false;
        public static bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                if (isActive)
                {
                    GField.Panel.Children.Add(verticalLine);
                    GField.Panel.Children.Add(horizontalLine);
                }
                else
                {
                    GField.Panel.Children.Remove(verticalLine);
                    GField.Panel.Children.Remove(horizontalLine);
                }
            }
        }
        private static Line verticalLine = new() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 1 };
        private static Line horizontalLine = new() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 1 };


    }
}
