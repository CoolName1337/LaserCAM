using LaserCAM.CAM;
using LaserCAM.CAM.GTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LaserCAM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartInitializing();
        }

        public void StartInitializing()
        {
            GField.Panel = canvasField;
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            GField.ZoomOn(e.Delta > 0 ? 1 : -1);
            scaleTextBlock.Text = Math.Round(GField.KSize*100).ToString() + "%";
            scaleSlider.Value = GField.KSize * 100;
        }

        private void scaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GField.KSize = e.NewValue/100;
            scaleTextBlock.Text = Math.Round(GField.KSize * 100).ToString() + "%";
        }
        /////////////////////////////////////////////////// Canvas mouse input ////////////////////////////////////////////////////////////////////////////////////
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                GField.PastPos = e.GetPosition(mainCanvas);
            }

            GCursor.SelectedTool?.OnMouseDown(sender, e);
        }
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GCursor.SelectedTool?.OnMouseUp(sender, e);
        }
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            GCursor.Position = e.GetPosition(mainCanvas);

            XTextBox.Text = GCursor.Position.X.ToString();
            YTextBox.Text = GCursor.Position.Y.ToString();

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                GField.MoveCanvas(e.GetPosition(mainCanvas));
            }

            GCursor.SelectedTool?.OnMouseMove(sender, e);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button btn)
            {
                switch (btn.Tag)
                {
                    case "none":
                        break;
                    case "line":
                        GCursor.SelectedTool = new GToolLine();
                        break;
                    case "ellipse":
                        break;
                    case "rectangle":
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
