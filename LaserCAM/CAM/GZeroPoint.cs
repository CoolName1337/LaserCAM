using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM.CAM
{
    public static class GZeroPoint
    {
        private static Point _position;
        static GZeroPoint()
        {
            GField.Panel.Children.Add(verticalLine);
            GField.Panel.Children.Add(horizontalLine);
        }
        public static Point Position
        {
            get => _position;
            set
            {
                _position = value;

                verticalLine.X1 = verticalLine.X2 = _position.X;

                var height = GField.MainPanel.ActualHeight;
                var width = GField.MainPanel.ActualWidth;

                verticalLine.Y1 = 2000 + height*2 / GField.KSize - _position.Y;
                verticalLine.Y2 = -2000 - height*2 / GField.KSize - _position.Y;

                horizontalLine.Y1 = horizontalLine.Y2 = -_position.Y;

                horizontalLine.X1 = -2000 - width*2 / GField.KSize + _position.X;
                horizontalLine.X2 = 2000 + width*2 / GField.KSize + _position.X;

                GGrid.Reload();
            }
        }

        private static Line verticalLine = new() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 1 };
        private static Line horizontalLine = new() { Stroke = new SolidColorBrush(Colors.Lime), StrokeThickness = 1 };


    }
}
