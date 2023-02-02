using LaserCAM.CAM;
using LaserCAM.CAM.GTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LaserCAM
{
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
            GTool.ParamsWindow = ParamsContainer;
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

            XTextBlock.Text = GCursor.Position.X.ToString();
            YTextBlock.Text = GCursor.Position.Y.ToString();

            if(GTool.ParamsWindow.Child is Grid gr)
            {
                foreach(FrameworkElement element in gr.Children)
                {
                    if(element is TextBox tb)
                    {
                        if (tb.Tag == "x" || tb.Tag == "X1")
                            tb.Text = GCursor.Position.X.ToString();
                        if (tb.Tag == "y" || tb.Tag == "Y1")
                            tb.Text = GCursor.Position.Y.ToString();
                    }
                }
            }

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
                SelectButton(btn);
                switch (btn.Tag)
                {
                    case "none":
                        break;
                    case "line":
                        GCursor.SelectedTool = new GToolLine();
                        break;
                    case "ellipse":
                        GCursor.SelectedTool = new GToolEllipse();
                        break;
                    case "rectangle":
                        GCursor.SelectedTool = new GToolRectangle();
                        break;
                    default:
                        break;
                }
            }
        }

        private void SelectButton(Button btn)
        {
            foreach (Button toolBtn in ToolsContainer.Children)
                toolBtn.Background = new SolidColorBrush(Colors.Transparent);
            btn.Background = new SolidColorBrush(Colors.WhiteSmoke);
            GTool.ParamsWindow.Child = null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                GCursor.SelectedTool?.OnLeftMouseDown();
                if(e.Source is TextBox tb) e.Handled = true;
            }
            if (GTool.ParamsWindow.Child is Grid gr)
            {
                switch (e.Key)
                {
                    case Key.X:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "x" || el.Tag=="X1")
                                el.Focus();
                        break;
                    case Key.Y:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "y" || el.Tag == "Y1")
                                el.Focus();
                        break;
                    case Key.W:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "w")
                                el.Focus();
                        break;
                    case Key.H:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "h")
                                el.Focus();
                        break;
                    case Key.Escape:
                        GCursor.SelectedTool = null;
                        break;
                }
            }
            
        }
    }
}
