using LaserCAM.CAM.GTools;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LaserCAM
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            widthInput.Text = "100";
            heightInput.Text = "100";
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(widthInput.Text, out double w))
            {
                Canvas.SetLeft(sampleRectangle, canvas.ActualWidth / 2 - w / 2);
                sampleRectangle.Width = w;
            }
            if (double.TryParse(heightInput.Text, out double h))
            {
                Canvas.SetBottom(sampleRectangle, canvas.ActualHeight / 2 - h / 2);
                sampleRectangle.Height = h;
            }
        }

        private void input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Count() == 0) return;
            if (!char.IsDigit(e.Text[0]) && e.Text[0] != ',')
                e.Handled = true;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).SetSample(sampleRectangle.Width, sampleRectangle.Height);
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems[0] is TextBlock textBox)
            {

                switch (textBox.Text)
                {
                    case "A6":
                        widthInput.Text = "148";
                        heightInput.Text = "105";
                        break;
                    case "A5":
                        widthInput.Text = "210";
                        heightInput.Text = "148";
                        break;
                    case "A4":
                        widthInput.Text = "297";
                        heightInput.Text = "210";
                        break;
                    case "A3":
                        widthInput.Text = "420";
                        heightInput.Text = "297";
                        break;
                    case "A2":
                        widthInput.Text = "594";
                        heightInput.Text = "420";
                        break;
                    case "A1":
                        widthInput.Text = "841";
                        heightInput.Text = "594";
                        break;
                    case "A0":
                        widthInput.Text = "1189";
                        heightInput.Text = "841";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
