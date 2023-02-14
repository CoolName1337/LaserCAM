using LaserCAM.CAM.GShapes;
using System.Collections.Generic;
using System.Windows;
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
            if(e.ChangedButton == MouseButton.Left) 
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
                _line.X1 = _line.X2 = Cursor.X;
                _line.Y1 = _line.Y2 = -Cursor.Y;
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
            _line.X2 = Cursor.X;
            _line.Y2 = -Cursor.Y;
        }
        public override void RemoveShape()
        {
            GField.Panel.Children.Remove(_line);
        }


        public override void SetParamWindow()
        {
            base.SetParamWindow();

            if (ParamsWindow.Child is Grid gr)
            {
                List<FrameworkElement> elems = new();
                foreach (FrameworkElement el in gr.Children)
                {
                    if (el is TextBox && el.Tag == "d")
                        elems.Add(el);
                    if (el is TextBlock tb && tb.Text.StartsWith("D"))
                        elems.Add(el);
                }
                foreach (var el in elems)
                {
                    gr.Children.Remove(el);
                }
            }

        }

        public override void SetParams()
        {
            if(ClicksCount > 0)
            {
                _line.X2 = Cursor.X;
                _line.Y2 = -Cursor.Y;
            }
            else
            {
                _line.X1 = Cursor.X;
                _line.Y1 = -Cursor.Y;
            }

        }
    }
}
