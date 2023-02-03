using LaserCAM.CAM.GShapes;
using System.Windows;
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
            if (e.LeftButton == MouseButtonState.Pressed)
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
            foreach (TextBox textBox in ParamInputs)
            {
                if (double.TryParse(textBox.Text, out double res))
                {
                    switch (textBox.Tag)
                    {
                        case "w":
                            _ellipse.Width = res;
                            break;
                        case "h":
                            _ellipse.Height = res;
                            break;
                        case "x":
                            Canvas.SetLeft(_ellipse, res - _ellipse.Width / 2);
                            break;
                        case "y":
                            Canvas.SetBottom(_ellipse, res - _ellipse.Height / 2);
                            break;
                    }
                    textBox.BorderBrush = GrayBrush;
                }
                else
                    textBox.BorderBrush = RedBrush;
            }
        }

        public override void RemoveShape()
        {
            GField.Panel.Children.Remove(_ellipse);
        }
    }
}
