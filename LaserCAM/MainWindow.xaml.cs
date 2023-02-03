using LaserCAM.CAM;
using LaserCAM.CAM.GTools;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
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
            GField.MainPanel = mainCanvas;
            GField.Panel = canvasField;
            GTool.ParamsWindow = ParamsContainer;
            UnsetSample();
        }

        public void SetSample(double w, double h, Point ZeroPoint = new Point())
        {
            UnsetSample();
            GField.Sample = new GSample(w, h);
            GPoint.IsActive = true;
            GPoint.Position = ZeroPoint;
            GField.MoveCanvas(new Point(mainCanvas.ActualWidth / 2, mainCanvas.ActualHeight/2));

            ViewContainer.Visibility = Visibility.Visible;
            ToolsContainer.Visibility = Visibility.Visible;
            ParamsContainer.Visibility = Visibility.Visible;
            StartMask.Visibility = Visibility.Hidden;
        }

        public void UnsetSample()
        {
            GField.Clear();
            ViewContainer.Visibility = Visibility.Hidden;
            ToolsContainer.Visibility = Visibility.Hidden;
            ParamsContainer.Visibility = Visibility.Hidden;
            StartMask.Visibility = Visibility.Visible;
            GPoint.IsActive = false;
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            GField.ZoomOn(e.Delta > 0 ? 1 : -1);
            scaleTextBlock.Text = Math.Round(GField.KSize * 100).ToString() + "%";
            scaleSlider.Value = GField.KSize * 100;
        }

        private void scaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GField.KSize = e.NewValue / 100;
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
            var mousePos = e.GetPosition(mainCanvas);
            GCursor.Position = mousePos;

            XTextBlock.Text = GCursor.Position.X.ToString();
            YTextBlock.Text = GCursor.Position.Y.ToString();

            if (GTool.ParamsWindow.Child is Grid gr)
            {
                foreach (FrameworkElement element in gr.Children)
                {
                    if (element is TextBox tb)
                    {
                        if (tb.Tag == "x")
                            tb.Text = GCursor.Position.X.ToString();
                        if (tb.Tag == "y")
                            tb.Text = GCursor.Position.Y.ToString();
                    }
                }
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                GField.MoveCanvas(mousePos);
            }

            GCursor.SelectedTool?.OnMouseMove(sender, e);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                SelectButton(btn);
                switch (btn.Tag)
                {
                    case "cursor":
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
            if (e.Key == Key.Space)
            {
                GCursor.SelectedTool?.OnLeftMouseDown();
                if (e.Source is TextBox tb) e.Handled = true;
            }
            if (GTool.ParamsWindow.Child is Grid gr)
            {
                switch (e.Key)
                {
                    case Key.X:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "x")
                                el.Focus();
                        break;
                    case Key.Y:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "y")
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

        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            var wind = new StartWindow();
            wind.Show();
        }

        private async void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Filter = "LCAM files (*.lcam)|*.lcam" };
            if (openFileDialog.ShowDialog() == true)
            {
                await FileExtensioner.OpenProject(openFileDialog.FileName);
            }
        }
        private async void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog() { Filter = "LCAM files (*.lcam)|*.lcam" };
            if (saveFileDialog.ShowDialog() == true)
            {
                await FileExtensioner.SaveProject(saveFileDialog.FileName);
            }
        }
    }
}
