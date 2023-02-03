using LaserCAM.CAM.GTools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LaserCAM.CAM
{
    public class GSample
    {
        Rectangle _rectangle;

        public GSample(double w, double h)
        {
            _rectangle = new Rectangle() { Width = w, Height = h, Stroke=GTool.BlueBrush, StrokeThickness=1};
            Position = new Point(0, 0);
            GField.Panel.Children.Add(_rectangle);
        }

        private Point _position;
        public Point Position { 
            get => _position;
            set
            {
                _position = value;
                Canvas.SetBottom(_rectangle, value.Y);
                Canvas.SetLeft(_rectangle, value.X);
            }
        }

        public double Width { get => _rectangle.Width; }
        public double Height { get => _rectangle.Height; }

        public void Remove()
        {
            GField.Panel.Children.Remove(_rectangle);
        }
    }
}
