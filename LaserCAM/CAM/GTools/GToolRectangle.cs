using LaserCAM.CAM.GShapes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GTools
{
    internal class GToolRectangle : GTool
    {
        Rectangle _rectangle;
        Point point1;
        public override void InitializeShape()
        {
            _rectangle = new Rectangle() { StrokeThickness = 1, Stroke = GrayBrush };
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
            if (ClicksCount > 0)
            {
                GRectangle GRectangle = new GRectangle(_rectangle);
                GRectangle.Create();
                InitializeShape();
                ClicksCount = 0;
            }
            else
            {
                point1 = Cursor;
                _rectangle.Width = 0;
                _rectangle.Height = 0;
                Canvas.SetTop(_rectangle, Cursor.Y);
                Canvas.SetLeft(_rectangle, Cursor.X);
                GField.Panel.Children.Add(_rectangle);
                ClicksCount++;
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            var point2 = Cursor - point1;

            _rectangle.Height = Math.Abs(point2.Y);
            _rectangle.Width = Math.Abs(point2.X);

            if (point2.Y < 0)
                Canvas.SetTop(_rectangle, Cursor.Y);
            if (point2.X < 0)
                Canvas.SetLeft(_rectangle, Cursor.X);
        }

        public override void RemoveShape()
        {
            GField.Panel.Children.Remove(_rectangle);
        }


        public override void SetParamWindow()
        {
            base.SetParamWindow();

            if(ParamsWindow.Child is Grid gr)
            {
                List<FrameworkElement> elems = new();
                foreach (FrameworkElement el in gr.Children)
                {
                    if(el is TextBox && (el.Tag == "h" || el.Tag == "w"))
                        elems.Add(el);
                    if(el is TextBlock tb && (tb.Text.StartsWith("W") || tb.Text.StartsWith("H")))
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
            foreach (TextBox textBox in ParamInputs)
            {
                if (double.TryParse(textBox.Text, out double res))
                {
                    var point2 = Cursor - point1;

                    _rectangle.Height = Math.Abs(point2.Y);
                    _rectangle.Width = Math.Abs(point2.X);

                    if (point2.Y < 0)
                        Canvas.SetTop(_rectangle, Cursor.Y);
                    if (point2.X < 0)
                        Canvas.SetLeft(_rectangle, Cursor.X);
                    textBox.BorderBrush = GrayBrush;
                }
                else
                    textBox.BorderBrush = RedBrush;
            }
        }
    }
}
