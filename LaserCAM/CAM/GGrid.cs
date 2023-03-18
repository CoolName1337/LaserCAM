using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM.CAM
{
    public static class GGrid
    {
        static List<Line> verticalLines = new();
        static List<Line> horizontalLines = new();

        static double _step = 1;
        public static double Step
        {
            get => _step;
            set
            {
                _step = value < 0 ? 0 : value;
                if (value == 0)
                {
                    IsActive = false;
                    return;
                }
                verticalLines.Clear();
                horizontalLines.Clear();
                for (double i = GZeroPoint.Position.X; i < GField.MainPanel.ActualWidth; i += _step)
                    horizontalLines.Add(CreateLine(i, i, GField.MainPanel.ActualHeight, -GField.MainPanel.ActualHeight));
                for (double i = -GZeroPoint.Position.Y; i < GField.MainPanel.ActualHeight; i += _step)
                    verticalLines.Add(CreateLine(GField.MainPanel.ActualWidth, -GField.MainPanel.ActualWidth, i, i));

                for (double i = GZeroPoint.Position.X; i > -GField.MainPanel.ActualWidth; i -= _step)
                    horizontalLines.Add(CreateLine(i, i, GField.MainPanel.ActualHeight, -GField.MainPanel.ActualHeight));
                for (double i = -GZeroPoint.Position.Y; i > -GField.MainPanel.ActualHeight; i -= _step)
                    verticalLines.Add(CreateLine(GField.MainPanel.ActualWidth, -GField.MainPanel.ActualWidth, i, i));
            }
        }

        public static void Reload()
        {
            verticalLines.ForEach(line => GField.Panel.Children.Remove(line));
            horizontalLines.ForEach(line => GField.Panel.Children.Remove(line));
            Step = Step;
            IsActive = IsActive;
        }

        private static Line CreateLine(double x1, double x2, double y1, double y2) => new Line
        {
            Stroke = new SolidColorBrush(Color.FromArgb(10, 0, 0, 0)),
            StrokeThickness = 1,
            X1 = x1,
            X2 = x2,
            Y1 = y1,
            Y2 = y2
        };

        private static bool _isActive = false;
        public static bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (value)
                {
                    verticalLines.ForEach(line => GField.Panel.Children.Add(line));
                    horizontalLines.ForEach(line => GField.Panel.Children.Add(line));
                }
                else
                {
                    verticalLines.ForEach(line => GField.Panel.Children.Remove(line));
                    horizontalLines.ForEach(line => GField.Panel.Children.Remove(line));
                }
            }
        }


        static GGrid()
        {
            Step = 10;
        }
    }
}
