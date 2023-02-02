using LaserCAM.CAM.GShapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GTools
{
    public class GToolLine : GTool
    {
        Line _line;
        public override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) 
            {
                OnLeftMouseDown();
            }
        }
        public override void OnLeftMouseDown()
        {
            if (ClicksCount > 0)
            {
                GLine Gline = new GLine(_line);
                Gline.Create();
                InitializeShape();
                ClicksCount = 0;
            }
            else
            {
                GField.Panel.Children.Add(_line);
                _line.X1 = _line.X2 = GCursor.Position.X;
                _line.Y1 = _line.Y2 = GCursor.Position.Y;
                ClicksCount++;
            }
        }

        public override void InitializeShape()
        { 
            _line = new Line() { Stroke = GrayBrush, StrokeThickness = 1 }; 
            SetParams();
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            _line.X2 = GCursor.Position.X;
            _line.Y2 = GCursor.Position.Y;
        }
        public override void RemoveShape()
        {
            GField.Panel.Children.Remove(_line);
        }

        public override void SetParams()
        {

        }
    }
}
