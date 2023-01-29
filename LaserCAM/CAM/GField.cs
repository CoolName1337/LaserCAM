using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LaserCAM.CAM
{
    public static class GField
    {
        private static double scaleStep = 1 / 10d;
        private static double kSize = 1;
        public static double KSize
        {
            get => kSize; 
            set
            {
                kSize = value < 0.3 ? 0.3 : value > 3 ? 3 : value;
                kSize = Math.Round(kSize, 1);
                if (Panel != null)
                    Panel.RenderTransform = new ScaleTransform(kSize, kSize);
            }
        }
        public static Point Position
        {
            get => new Point(Canvas.GetLeft(Panel), Canvas.GetTop(Panel));
            set
            {
                Canvas.SetLeft(Panel, value.X);
                Canvas.SetTop(Panel, value.Y);
            }
        }
        private static Panel _panel;
        public static Panel Panel
        {
            get => _panel;
            set
            {
                _panel = value;
                Canvas.SetTop(_panel, 0);
                Canvas.SetLeft(_panel, 0);
            }
        }
        public static Point PastPos { get; set; } = new Point();

        public static void ZoomOn(double kStep)
        {
            KSize += scaleStep * kStep;
        }

        public static void MoveCanvas(Point point)
        {
            var delta = PastPos - point;

            Position -= new Vector(delta.X, delta.Y);

            PastPos = point;
        }

    }
}
