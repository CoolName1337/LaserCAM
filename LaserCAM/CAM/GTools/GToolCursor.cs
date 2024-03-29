﻿using LaserCAM.CAM.GShapes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace LaserCAM.CAM.GTools
{
    public class GToolCursor : GTool
    {
        Rectangle selectRectangle = new Rectangle
        {
            Stroke = GTool.GrayBrush,
            StrokeThickness = 1,
            StrokeDashArray = new System.Windows.Media.DoubleCollection() { 6, 6 },
            StrokeDashCap = System.Windows.Media.PenLineCap.Round
        };


        private bool IsInsideSelectedArea(Point p)
        {
            var pX = Canvas.GetLeft(selectRectangle);
            var pY = Canvas.GetBottom(selectRectangle);
            return p.X > pX && p.X < pX + selectRectangle.Width && p.Y > pY && p.Y < pY + selectRectangle.Height;
        }

        private bool IsInsideSelectedArea(Point p, double width, double height)
        {
            var pX = Canvas.GetLeft(selectRectangle);
            var pY = Canvas.GetBottom(selectRectangle);
            return p.X + width/2 > pX && p.X - width/2 < pX + selectRectangle.Width &&
                p.Y + height/2 > pY && p.Y - height/2 < pY + selectRectangle.Height;
        }

        Point point1;
        public override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                OnLeftMouseDown();
            }
        }
        public override void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if(!Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                GField.ClearSelect();
            }
            foreach (var gShape in GField.AllShapes)
            {
                if(gShape is GImage gImage)
                {
                    var p = new Point(
                        Canvas.GetLeft(gImage.Image) + gImage.Image.Width / 2,
                        Canvas.GetBottom(gImage.Image) + gImage.Image.Height / 2
                        );
                    if (IsInsideSelectedArea(p, gImage.Image.Width, gImage.Image.Height))
                        GField.ChangeSelecting(gShape);
                }
                else
                {
                    foreach (var gBindingPoint in gShape.GetBindingPoints())
                    {
                        if (IsInsideSelectedArea(gBindingPoint.Point))
                        {
                            GField.ChangeSelecting(gShape);
                            break;
                        }
                    }
                }
            }
            GField.Panel.Children.Remove(selectRectangle);
            SetParamWindowStyle();
        }
        public override void OnLeftMouseDown()
        {
            point1 = GCursor.Position;
            selectRectangle.Width = 0;
            selectRectangle.Height = 0;
            Canvas.SetBottom(selectRectangle, GCursor.Position.Y);
            Canvas.SetLeft(selectRectangle, GCursor.Position.X);
            GField.Panel.Children.Add(selectRectangle);
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed)
            {
                GField.Panel.Children.Remove(selectRectangle);
                return;
            }
            var point2 = GCursor.Position - point1;

            selectRectangle.Height = Math.Abs(point2.Y);
            selectRectangle.Width = Math.Abs(point2.X);
            selectRectangle.StrokeThickness = 1/GField.KSize;

            if (point2.Y < 0)
                Canvas.SetBottom(selectRectangle, GCursor.Position.Y);
            if (point2.X < 0)
                Canvas.SetLeft(selectRectangle, GCursor.Position.X);

        }

        private void SetParamWindowStyle()
        {
            ParamsWindow.Visibility = Visibility.Hidden;
        }
        public override void SetParamWindow() { SetParamWindowStyle(); }

        public override void InitializeShape() { }

        public override void RemoveShape() { }

        public override void SetParams() { }
    }
}
