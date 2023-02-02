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
        public static List<TextBox> ParamInputs
        {
            get
            {
                List<TextBox> boxes = new();
                foreach (FrameworkElement el in (ParamsWindow.Child as Grid)?.Children)
                    if (el is TextBox tb)
                        boxes.Add(tb);
                return boxes;
            }
        }

        public static Point Cursor
        {
            get
            {
                if(double.TryParse(ParamInputs.Find(el => el.Tag == "x").Text, out double x)){ }
                if(double.TryParse(ParamInputs.Find(el => el.Tag == "y").Text, out double y)){ }
                return new Point(x, y);
            }
        }

        public static Brush GrayBrush { get => new SolidColorBrush(Colors.Gray); }
        public static Brush BlackBrush { get => new SolidColorBrush(Colors.Black); }
        public static Brush RedBrush { get => new SolidColorBrush(Colors.Red); }

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


            var tbk = new TextBlock() { Text = "W:" };
            Grid.SetColumn(tbk, 0);
            Grid.SetRow(tbk, 2);
            grid.Children.Add(tbk);
            tbk = new TextBlock() { Text = "H:" };
            Grid.SetColumn(tbk, 2);
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

            var tbx = new TextBox() { Tag = "w", Text = "20" };
            Grid.SetColumn(tbx, 1);
            Grid.SetRow(tbx, 2);
            grid.Children.Add(tbx);
            tbx = new TextBox() { Tag = "h", Text = "20" };
            Grid.SetColumn(tbx, 3);
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
