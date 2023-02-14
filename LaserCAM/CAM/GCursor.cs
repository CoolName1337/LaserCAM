using LaserCAM.CAM.GTools;
using LaserCAM.Extensions;
using System;
using System.Windows;
using System.Windows.Shapes;

namespace LaserCAM.CAM
{

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
                else
                {
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

        private static double _step = 0.01;
        public static double Step
        {
            get => _step;
            set => _step = value > 0.01 ? value : 0.01;
        }

        public static bool UseStep { get; set; } = false;

        private static Point _position;

        /// <summary>
        /// Specially for shape positioning
        /// </summary>
        public static Point Position
        {
            get => _position;
            set
            {
                // Set calculated position

                var point = (new Point(-GField.Position.X, GField.Position.Y) - new Vector(-value.X, value.Y)).Divide(GField.KSize);

                _position = new Point(Math.Round(point.X, 2), Math.Round(point.Y, 2));
                if (UseStep || Step == 0.01)
                    _position = new Point(
                        _position.X + GPoint.Position.X % Step - _position.X % Step, 
                        _position.Y + GPoint.Position.Y % Step - _position.Y % Step
                        );

                // Set position for aim

                verticalLine.X1 = verticalLine.X2 = _position.X;

                verticalLine.Y1 = GField.MainPanel.ActualHeight / GField.KSize - _position.Y;
                verticalLine.Y2 = -GField.MainPanel.ActualHeight / GField.KSize - _position.Y;

                horizontalLine.Y1 = horizontalLine.Y2 = -_position.Y;

                horizontalLine.X1 = -GField.MainPanel.ActualWidth / GField.KSize + _position.X;
                horizontalLine.X2 = GField.MainPanel.ActualWidth / GField.KSize + _position.X;

            }
        }
        /// <summary>
        /// Specially for outputs (for textBoxes, for results and etc)
        /// </summary>
        public static Point RelativePosition
        {
            get => (Point)(Position - GPoint.Position);
        }

    }
}
