using LaserCAM.CAM;
using LaserCAM.CAM.GTools;
using Microsoft.Win32;
using LaserCAM.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LaserCAM
{
    public partial class MainWindow : Window
    {
        public static RoutedCommand ReturnCommand = new RoutedCommand();

        public MainWindow()
        {
            foreach (string key in GBindingParams.Params.Keys)
                Resources.Add(key, GBindingParams.Params[key]);
            InitializeComponent();
            StartInitializing();
        }

        public void StartInitializing()
        {
            GField.MainPanel = mainCanvas;
            GField.Panel = canvasField;
            GTool.ParamsWindow = ParamsContainer;
            GCursor.ViewConatiner = ViewContainer;

            ReturnCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
        }

        private void ClearTool()
        {
            SelectButton(ToolsContainer.Children[0] as Button);
            GCursor.SelectedTool = new GToolCursor();
        }

        public void SetSample(double w, double h, Point ZeroPoint = new Point())
        {
            UnsetSample();
            GField.Sample = new GSample(w, h);
            GZeroPoint.IsActive = true;
            GZeroPoint.Position = ZeroPoint;
            GField.MoveCanvas(new Point(mainCanvas.ActualWidth / 2, mainCanvas.ActualHeight / 2));

            ViewContainer.Visibility = Visibility.Visible;
            ToolsContainer.Visibility = Visibility.Visible;
            ParamsContainer.Visibility = Visibility.Visible;
            FieldParamsPanel.Visibility = Visibility.Visible;
            StartMask.Visibility = Visibility.Hidden;

            ClearTool();
        }

        public void UnsetSample()
        {
            GField.Clear();
            ViewContainer.Visibility = Visibility.Hidden;
            ToolsContainer.Visibility = Visibility.Hidden;
            ParamsContainer.Visibility = Visibility.Hidden;
            FieldParamsPanel.Visibility = Visibility.Hidden;
            StartMask.Visibility = Visibility.Visible;
            GZeroPoint.IsActive = false;
            GGrid.IsActive = false;
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
            GCursor.SetPositionFromMouse(mousePos);

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
                        GCursor.SelectedTool = new GToolCursor();
                        break;
                    case "line":
                        GCursor.SelectedTool = new GToolLine();
                        break;
                    case "ellipse":
                        GCursor.SelectedTool = new GToolEllipse();
                        break;
                    case "arc":
                        GCursor.SelectedTool = new GToolArc();
                        break;
                    case "rectangle":
                        GCursor.SelectedTool = new GToolRectangle();
                        break;
                    case "image":
                        var openFileDialog = new OpenFileDialog() { Filter = "Png |*.png|JPG|*.jpeg" };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            GCursor.SelectedTool = new GToolImage(openFileDialog.FileName);
                        }
                        else
                        {
                            ClearTool();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void SelectButton(Button btn)
        {
            if (btn == null) return;
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
                e.Handled = true;
            }
            if (e.Key == Key.Delete)
            {
                GField.RemoveSelected();
            }
            if (e.Key == Key.O)
            {
                GZeroPoint.Position = GCursor.Position;
            }

            if (e.Key == Key.Right)
                GCursor.Position += new Vector(GCursor.Step, 0);
            if (e.Key == Key.Left)
                GCursor.Position += new Vector(-GCursor.Step, 0);
            if (e.Key == Key.Up)
                GCursor.Position += new Vector(0, GCursor.Step);
            if (e.Key == Key.Down)
                GCursor.Position += new Vector(0, -GCursor.Step);

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
                    case Key.D:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "d")
                                el.Focus();
                        break;
                    case Key.R:
                        foreach (FrameworkElement el in gr.Children)
                            if (el.Tag == "r")
                                el.Focus();
                        break;
                    case Key.F:
                        if(GCursor.SelectedTool is GToolArc gtarc)
                        {
                            gtarc.Flip();
                        }
                        break;
                    case Key.Escape:
                        ClearTool();
                        break;
                }
            }

        }

        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            var wind = new StartWindow();
            wind.Show();
        }
        
        private void OpenParam_Click(object sender, RoutedEventArgs e)
        {
            var wind = new ParamsWindow();
            wind.Show();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Filter = "LCAM files (*.lcam)|*.lcam" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileExtensioner.OpenProject(openFileDialog.FileName);
            }
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog() { Filter = "LCAM files (*.lcam)|*.lcam" };
            if (saveFileDialog.ShowDialog() == true)
            {
                FileExtensioner.SaveProject(saveFileDialog.FileName);
            }
        }

        private void GridParams_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb)
            {
                switch (cb.Tag)
                {
                    case "aim":
                        GCursor.IsAim = cb.IsChecked ?? false;
                        break;
                    case "step":
                        GCursor.UseStep = cb.IsChecked ?? false;
                        break;
                    case "grid":
                        GGrid.IsActive = cb.IsChecked ?? false;
                        break;
                }
            }
        }

        private void GCodeCreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (GField.AllShapes.Count > 0)
            {
                GCodeTextBox.Text = FileExtensioner.GenerateGCode();
            }
        }

        private void StepSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!char.IsDigit(e.Text[0]) && e.Text != ",")
                    e.Handled = true;
            }
        }

        private void StepSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (double.TryParse(textBox.Text, out double res))
                {
                    GCursor.Step = res;
                    textBox.BorderBrush = GTool.GrayBrush;
                }
                else
                    textBox.BorderBrush = GTool.RedBrush;
            }
        }

        private void ReturnCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var change = GField.Changes.LastOrDefault();
            change?.Return();
            GField.Changes.Remove(change);
        }

        private void GridBinding_Click(object sender, RoutedEventArgs e)
        {
            if(e.Source is CheckBox cb)
            {
                if (cb.Tag.ToString() == "UseBinding")
                    GCursor.SetBindingPointVisibility(Visibility.Hidden);
                GBindingParams.Params[cb.Tag.ToString()] = cb.IsChecked ?? false;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show(
                "O - Переместить нулевую точку\n" +
                "Ctrl+Z - Назад\n" +
                "← ↑ → ↓  - Переместить курсор\n" +
                "DELETE - Удалить выделенные фигуры\n" +
                "X, Y - Соответствующие оси\n" +
                "R, D - Радиус, диаметр(размер картинки)\n" +
                "F - Разворот дуги\n");
        }
    }
}