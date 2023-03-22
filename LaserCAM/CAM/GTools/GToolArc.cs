using LaserCAM.CAM.GShapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GTools
{
    public class GToolArc : GTool
    {
        bool isFlipped;
        Path path;
        PathFigure arcFigure;
        ArcSegment arcSegment;
        public override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                OnLeftMouseDown();
            }
        }
        public override void OnLeftMouseDown()
        {
            ClicksCount++;
            if (ClicksCount > 1)
            {
                GArc garc = new(path, arcFigure.StartPoint, arcSegment.Point, arcSegment.Size.Height);
                garc.Create();
                InitializeShape();
                ClicksCount = 0;
            }
            else if (ClicksCount > 0)
            {
                arcFigure.StartPoint = new Point(Cursor.X, -Cursor.Y);
                arcSegment.Point = new Point(Cursor.X, -Cursor.Y);
                GField.Panel.Children.Add(path);
            }
        }
        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isFlipped)
                arcFigure.StartPoint = new Point(Cursor.X, -Cursor.Y);
            else
                arcSegment.Point = new Point(Cursor.X, -Cursor.Y);
        }

        public override void InitializeShape()
        {
            path = new() { Stroke = GrayBrush, StrokeThickness = 1 };
            arcSegment = new ArcSegment() { Point = new Point(0, 0), Size = new Size(30, 30) };
            arcFigure = new PathFigure(
                new Point(0, 0),
                new List<PathSegment>() { arcSegment },
                false
            );
            var figures = new PathFigureCollection() { arcFigure };
            path.Data = new PathGeometry() { Figures = figures };
        }

        public override void RemoveShape()
        {
            GField.Panel.Children.Remove(path);
        }

        public override void SetParams()
        {
            arcSegment.Size = new Size(ParamValues["r"], ParamValues["r"]);
            if(isFlipped)
                arcFigure.StartPoint = new Point(Cursor.X, -Cursor.Y);
            else
                arcSegment.Point = new Point(Cursor.X, -Cursor.Y);
        }

        public override void SetParamWindow()
        {
            base.SetParamWindow(); 
            if (ParamsWindow.Child is Grid gr)
            {
                foreach (FrameworkElement el in gr.Children)
                {
                    if(el is TextBox textBox && textBox.Tag == "d")
                        textBox.Tag = "r";
                    if (el is TextBlock tb && tb.Text.StartsWith("D"))
                        tb.Text = "R:";
                }
                TextBlock textBlock = new TextBlock() { Text = "F" };
                Grid.SetColumn(textBlock, 2);
                Grid.SetRow(textBlock, 2);
                gr.Children.Add(textBlock);

                Button btnFlip = new Button() { Content = "Развернуть" };
                Grid.SetColumn(btnFlip, 3);
                Grid.SetRow(btnFlip, 2);
                btnFlip.Click += BtnFlip_Click;
                gr.Children.Add(btnFlip);
            }

        }

        public void Flip()
        {
            isFlipped = !isFlipped;
            var point = arcSegment.Point;
            arcSegment.Point = arcFigure.StartPoint;
            arcFigure.StartPoint = point;
        }

        private void BtnFlip_Click(object sender, RoutedEventArgs e)
        {
            Flip();
        }
    }
}
