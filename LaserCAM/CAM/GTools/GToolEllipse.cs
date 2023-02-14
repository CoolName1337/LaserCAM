using LaserCAM.CAM.GShapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GTools
{
    public class GToolEllipse : GTool
    {
        Ellipse _ellipse;
        public override void InitializeShape()
        {
            _ellipse = new Ellipse() { Stroke = GrayBrush, StrokeThickness = 1 };

            SetParams();

            GField.Panel.Children.Add(_ellipse);
        }

        public override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                OnLeftMouseDown();
            }
        }
        public override void OnLeftMouseDown()
        {
            GEllipse Gellipse = new(_ellipse);
            Gellipse.Create();
            InitializeShape();
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetBottom(_ellipse, GCursor.Position.Y - _ellipse.Height / 2);
            Canvas.SetLeft(_ellipse, GCursor.Position.X - _ellipse.Width / 2);
        }


        public override void SetParams()
        {
            _ellipse.Width = _ellipse.Height = ParamValues["d"]; 
            Canvas.SetLeft(_ellipse, Cursor.X - _ellipse.Width / 2);
            Canvas.SetBottom(_ellipse, Cursor.Y - _ellipse.Height / 2);
        }

        public override void RemoveShape()
        {
            GField.Panel.Children.Remove(_ellipse);
        }
    }
}
