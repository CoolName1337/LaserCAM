using LaserCAM.CAM.GShapes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GTools
{
    public class GToolLine : GTool
    {
        Line _line = new Line() { Stroke = GrayBrush };
        int countClick = 0;
        public override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) {
                if (countClick > 0)
                {
                    GLine Gline = new GLine(_line);
                    GField.Panel.Children.Remove(_line);
                    Gline.Create();
                    _line = new Line() { Stroke = GrayBrush };
                    countClick = 0;
                }
                else
                {
                    GField.Panel.Children.Add(_line);
                    _line.X1 = _line.X2 = GCursor.Position.X;
                    _line.Y1 = _line.Y2 = GCursor.Position.Y;
                    countClick++;
                }
            }
            
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            _line.X2 = GCursor.Position.X;
            _line.Y2 = GCursor.Position.Y;
        }

        public override void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
