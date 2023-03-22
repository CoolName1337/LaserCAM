using LaserCAM.CAM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LaserCAM.Windows
{
    /// <summary>
    /// Логика взаимодействия для ParamsWindow.xaml
    /// </summary>
    public partial class ParamsWindow : Window
    {
        public ParamsWindow()
        {
            Resources.Add("useDots", GParams.UseDots);
            Resources.Add("useSpaces", GParams.UseSpacesInResult);
            Resources.Add("fullCode", GParams.FullCode);
            Resources.Add("feed", GParams.Feed.ToString());
            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is CheckBox checkBox)
            {
                switch (checkBox.Tag)
                {
                    case "spaces":
                        GParams.UseSpacesInResult = checkBox.IsChecked ?? false;
                        break;
                    case "sep":
                        GParams.UseDots = checkBox.IsChecked ?? false;
                        break;
                    case "full":
                        GParams.FullCode = checkBox.IsChecked ?? false;
                        break;
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (e.Text == "." || e.Text == ",")
                    e.Handled = tb.Text.Contains('.') || tb.Text.Contains(',');
                else
                    e.Handled = !char.IsDigit(e.Text[0]);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && double.TryParse(tb.Text, out double res))
            {
                GParams.Feed = res;
            }
        }
    }
}
