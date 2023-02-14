using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LaserCAM.CAM.GTools
{
    public abstract class GTool
    {
        protected int ClicksCount = 0;
        public GTool()
        {
            SetParamWindow();
            InitializeShape();
        }
        public static Border ParamsWindow { get; set; }
        public static Dictionary<string, double> ParamValues
        {
            get
            {
                var dict = new Dictionary<string, double>();
                foreach (FrameworkElement el in (ParamsWindow.Child as Grid)?.Children)
                {
                    if(el is TextBox tb)
                    {
                        if(double.TryParse(tb.Text, out double val))
                            tb.BorderBrush = new SolidColorBrush(Colors.Gray);
                        else
                            tb.BorderBrush = new SolidColorBrush(Colors.Red);
                        dict.Add(tb.Tag?.ToString(), val);
                    }
                }
                return dict;
            }
        }

        public static Point Cursor
        {
            get => new Point(ParamValues["x"] + GPoint.Position.X, ParamValues["y"] + GPoint.Position.Y);
        }

        public static Brush GrayBrush { get => new SolidColorBrush(Colors.Gray); }
        public static Brush BlackBrush { get => new SolidColorBrush(Colors.Black); }
        public static Brush RedBrush { get => new SolidColorBrush(Colors.Red); }
        public static Brush BlueBrush { get => new SolidColorBrush(Colors.DarkBlue); }

        public virtual void OnMouseDown(object sender, MouseButtonEventArgs e) { }
        public virtual void OnLeftMouseDown() { }
        public virtual void OnMouseUp(object sender, MouseButtonEventArgs e) { }
        public virtual void OnMouseMove(object sender, MouseEventArgs e) { }

        public abstract void InitializeShape();

        public virtual void SetParamWindow()
        {
            // Sorry((((((((((((
            Grid grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());


            var tbk = new TextBlock() { Text = "D:" };
            Grid.SetColumn(tbk, 0);
            Grid.SetRow(tbk, 2);
            grid.Children.Add(tbk);
            tbk = new TextBlock() { Text = "X:" };
            Grid.SetColumn(tbk, 0);
            Grid.SetRow(tbk, 3);
            grid.Children.Add(tbk);
            tbk = new TextBlock() { Text = "Y:" };
            Grid.SetColumn(tbk, 2);
            Grid.SetRow(tbk, 3);
            grid.Children.Add(tbk);

            var tbx = new TextBox() { Tag = "d", Text = "20" };
            Grid.SetColumn(tbx, 1);
            Grid.SetRow(tbx, 2);
            grid.Children.Add(tbx);
            tbx = new TextBox() { Tag = "x", Text = GCursor.Position.X.ToString() };
            Grid.SetColumn(tbx, 1);
            Grid.SetRow(tbx, 3);
            grid.Children.Add(tbx);
            tbx = new TextBox() { Tag = "y", Text = GCursor.Position.Y.ToString() };
            Grid.SetColumn(tbx, 3);
            Grid.SetRow(tbx, 3);
            grid.Children.Add(tbx);

            foreach (FrameworkElement el in grid.Children)
                if (el is TextBox _tbx)
                {
                    _tbx.PreviewTextInput += DigitValidateInput;
                    _tbx.TextChanged += TextChaged;
                }

            ParamsWindow.Child = grid;
            ParamsWindow.Visibility = Visibility.Visible;
        }

        protected virtual void DigitValidateInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Count() == 0) return;
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != '-' && e.Text[0] != ',')
                e.Handled = true;
        }

        protected void TextChaged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SetParams();
            }
        }
        public abstract void SetParams();
        public abstract void RemoveShape();
    }
}
